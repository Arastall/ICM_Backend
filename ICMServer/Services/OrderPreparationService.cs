using DocumentFormat.OpenXml.InkML;
using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Managers;
using ICMServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ICMServer.Services
{
    public interface IOrderPreparationService
    {
        Task<List<OrdersToProcess>> PrepareOrdersToProcessCalculationsAsync();
    }

    public class OrderPreparationService : IOrderPreparationService
    {
        private readonly IPeriodContext _periodContext;
        private readonly ILogger<OrderPreparationService> _logger;
        private readonly IServiceProvider _sp;
        private readonly IHubContext<NotificationHub> _hub;


        public OrderPreparationService(
            IPeriodContext periodContext,
            ILogger<OrderPreparationService> logger, IServiceProvider sp,
            IHubContext<NotificationHub> hub)
        {
            _periodContext = periodContext;
            _logger = logger;
            _sp = sp;
            _hub = hub;
        }

        public async Task<List<OrdersToProcess>> PrepareOrdersToProcessCalculationsAsync()
        {
            try
            {
                var context = _sp.GetRequiredService<ICMDBContext>();
                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "preparing_orders",
                    stepName = "Preparing orders to threat",
                    status = "in_progress",
                    message = "Preparing Orders...",
                    time = DateTime.Now
                });

                // Delete existing orders to process
                //await context.Database.ExecuteSqlRawAsync("DELETE FROM OrdersToProcess");

                // Pre-fetch/Define the subqueries for better readability
                var skippedOrderIds = context.DataCancelReplaceSkippedOrders.Select(s => s.OrderRowId);

                // Pre-fetch valid payplan types (décommenté!)
                var validPayPlans = await context.DataPerformanceAgainstTargetMatrices
                    .Where(p => string.Compare(periodMonth, p.PeriodMonthStart.Value.ToString()) >= 0
                             && string.Compare(periodMonth, p.PeriodMonthEnd.Value.ToString()) <= 0
                             && p.PeriodYear.Value.ToString() == periodYear)
                    .Select(p => p.Payplan)
                    .Distinct()
                    .ToListAsync();

                // Main Query - CORRIGÉ
                var query = (from a in context.DataOrderHeaders
                             join b in context.DataOrderProcessHistories on a.OrderRowId equals b.RowId
                             join c in context.DataOrderPositions on a.OrderRowId equals c.OrderRowId
                             where b.RevenueProcessed == false  // Seulement RevenueProcessed, pas CommissionProcessed!
                                && a.PeriodYear == periodYear
                                && a.PeriodMonth == periodMonth
                                && !skippedOrderIds.Contains(a.OrderRowId)
                                && validPayPlans.Contains(c.PayplanType)  // Décommenté!
                             select a.OrderRowId).Distinct();

                var orderIds = await query.ToListAsync();

                var ordersWithDates = new List<OrdersToProcess>();

                foreach (var orderId in orderIds)
                {
                    var statusDate = await context.DataOrderStatusHistories
                        .Where(h => h.RowId == orderId
                            && (h.StatusCd == "Invoiced" || h.StatusCd == "Credited"
                                || h.StatusCd == "Order Despatched" || h.StatusCd == "Stock Issue"))
                        .OrderBy(h => h.StatusDt)
                        .Select(h => h.StatusDt)
                        .FirstOrDefaultAsync();

                    if (statusDate != default)
                    {
                        ordersWithDates.Add(new OrdersToProcess { OrderId = orderId, StatusDt = statusDate.Value });
                    }
                }

                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "preparing_orders",
                    stepName = "Preparing orders to threat",
                    status = "completed",
                    message = $"{ordersWithDates.Count} Order(s) to threat",
                    time = DateTime.Now
                });

                _logger.LogInformation("Prepared {Count} orders for processing", ordersWithDates.Count);

                return ordersWithDates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing orders to process calculations");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "preparing_orders",
                    stepName = "Preparing orders to threat",
                    status = "error",
                    message = $"Error occured: {ex.Message}",
                    time = DateTime.Now
                });
                throw;
            }
        }
    }
}
