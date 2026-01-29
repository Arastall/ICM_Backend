using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Managers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ICMServer.Services
{
    public interface ICalculationService
    {
        Task DoCalculationsAsync();
        Task DoCalculationsOldAsync();
    }

    public class CalculationService : ICalculationService
    {
        private readonly IOrderPreparationService _orderPreparation;
        private readonly ICreditAllocationService _creditAllocation;
        private readonly ICommissionPaymentService _commissionPayment;
        private readonly ISystemParametersHelper _sysParams;
        private readonly IPeriodContext _periodContext;
        private readonly ILogger<CalculationService> _logger;
        private readonly IServiceProvider _sp;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IProcessStateService _processState;

        public CalculationService(
            IOrderPreparationService orderPreparation,
            ICreditAllocationService creditAllocation,
            ICommissionPaymentService commissionPayment,
            ISystemParametersHelper sysParams,
            IPeriodContext periodContext,
            ILogger<CalculationService> logger,
            IServiceProvider sp,
            IHubContext<NotificationHub> hub,
            IProcessStateService processState)
        {
            _orderPreparation = orderPreparation;
            _creditAllocation = creditAllocation;
            _commissionPayment = commissionPayment;
            _periodContext = periodContext;
            _sysParams = sysParams;
            _logger = logger;
            _sp = sp;
            _hub = hub;
            _processState = processState;
        }

        public async Task DoCalculationsAsync()
        {
            try
            {
                var scope = _sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
                _logger.LogInformation("Do calculations - Started");

                _processState.UpdateStep("calculating", "Processing Calculations", "in_progress", "Starting calculations...");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "calculating",
                    stepName = "Processing Calculations",
                    status = "in_progress",
                    message = "Starting calculations...",
                    time = DateTime.Now
                });

                // Prepare orders to process
                var orders = await _orderPreparation.PrepareOrdersToProcessCalculationsAsync();

                int processedCount = 0;
                foreach (var order in orders)
                {
                    processedCount++;

                    // Check if order is authorized for commission
                    var isAuthorized = await IsOrderAuthorizedAsync(order.OrderId);

                    if (!isAuthorized)
                    {
                        _logger.LogDebug("Order {OrderId} not authorized", order.OrderId);
                        await _creditAllocation.SetRevenueProcessedFlagAsync(order.OrderId);
                        return;
                    }

                    var orderNumber = await context.DataOrderHeaders
                        .Where(h => h.OrderRowId == order.OrderId)
                        .Select(h => h.OrderNumber)
                        .FirstOrDefaultAsync();

                    var progressMessage = $"Calculating order {orderNumber} ({processedCount}/{orders.Count})";
                    _processState.UpdateStep("calculating", "Processing Calculations", "in_progress", progressMessage);
                    await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                    {
                        stepId = "calculating",
                        stepName = "Processing Calculations",
                        status = "in_progress",
                        message = progressMessage,
                        time = DateTime.Now
                    });

                    _logger.LogInformation("Order {Current}/{Total}: {OrderNumber} ({OrderId}) invoiced on {Date}",
                        processedCount, orders.Count, orderNumber, order.OrderId, order.StatusDt);

                    // Process order through all stages
                    await _creditAllocation.ProcessCreditAllocationOrderAsync(order.OrderId);
                    await _commissionPayment.ProcessCommissionPaymentAsync(order.OrderId);
                }

                _logger.LogInformation("Do calculations - Completed successfully");

                _processState.UpdateStep("calculating", "Processing Calculations", "completed", "Calculation completed");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "calculating",
                    stepName = "Processing Calculations",
                    status = "completed",
                    message = "Calculation completed",
                    time = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during calculations");
                _processState.UpdateStep("calculating", "Processing Calculations", "error", $"Error occurred: {ex.Message}");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "calculating",
                    stepName = "Processing Calculations",
                    status = "error",
                    message = $"Error occurred: {ex.Message}",
                    time = DateTime.Now
                });
                throw;
            }
        }

        private async Task<bool> IsOrderAuthorizedAsync(string orderId)
        {
            try
            {
                var scope = _sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

                var period = await context.ConfigSalesPeriods
                    .Where(p => p.PeriodYear == periodYear && p.PeriodMonth == periodMonth)
                    .Select(p => new { p.PeriodStart, p.PeriodEnd })
                    .FirstOrDefaultAsync();

                if (period == null)
                    return false;
                var lastStatus = await (
                    from sh in context.DataOrderStatusHistories
                    join oph in context.DataOrderProcessHistories on sh.RowId equals oph.RowId
                    where sh.RowId == orderId
                        && (sh.StatusDt >= period.PeriodStart && sh.StatusDt < period.PeriodEnd)
                    orderby sh.StatusDt descending
                    select new { sh.StatusCd, oph.BuName }
                ).FirstOrDefaultAsync();

                if (lastStatus == null)
                    return false;

                var commission = await context.ConfigOrderStatuses
                    .Where(c => c.BuName == lastStatus.BuName && c.StatusCd == lastStatus.StatusCd)
                    .Select(c => c.Commission)
                    .FirstOrDefaultAsync();

                return commission == true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if order is authorized");
                throw;
            }
        }

        public async Task DoCalculationsOldAsync()
        {
            try
            {
                _logger.LogInformation("Do Calculation old - Started");

                _processState.UpdateStep("do_calculation_old", "Calculating Figures (Legacy)", "in_progress", "Processing deals");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "do_calculation_old",
                    stepName = "Calculating Figures (Legacy)",
                    status = "in_progress",
                    message = "Processing deals",
                    time = DateTime.Now
                });

                var scope = _sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
                await context.Database.ExecuteSqlRawAsync("EXEC usp_RUN_CALCULATION_ENGINE");

                _logger.LogInformation("Do Calculation Old - Completed successfully");
                _processState.UpdateStep("do_calculation_old", "Calculating Figures (Legacy)", "completed", "Calculation completed");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "do_calculation_old",
                    stepName = "Calculating Figures (Legacy)",
                    status = "completed",
                    message = "Calculation completed",
                    time = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Calculation Old");
                _processState.UpdateStep("do_calculation_old", "Calculating Figures (Legacy)", "error", $"Error occurred: {ex.Message}");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "do_calculation_old",
                    stepName = "Calculating Figures (Legacy)",
                    status = "error",
                    message = $"Error occurred: {ex.Message}",
                    time = DateTime.Now
                });

                throw;
            }
        }
    }

    // DTO for OrdersToProcess table
    public class OrderToProcess
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public DateTime StatusDt { get; set; }
    }
}