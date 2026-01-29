
using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface IRollupAllocationService
    {
        Task RollupAllocationAsync(string orderId, int? specificAllocationId = null);
    }

    public class RollupAllocationService : IRollupAllocationService
    {
        private ICMDBContext _context;
        private readonly IPeriodContext _periodContext;
        private readonly ILogger<RollupAllocationService> _logger;
        private readonly IServiceProvider _sp;

        public RollupAllocationService(
            IPeriodContext periodContext,
            ILogger<RollupAllocationService> logger, IServiceProvider sp)
        {
            _periodContext = periodContext;
            _logger = logger;
            _sp = sp;
        }

        public async Task RollupAllocationAsync(string orderId, int? specificAllocationId = null)
        {
            try
            {
                using var scope = _sp.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

                // Get unprocessed allocations
                var query = _context.DataCreditAllocations
                    .Where(a => a.RollupProcessed == "N"
                        && a.AdjustmentId == null
                        && a.PeriodMonth == periodMonth
                        && a.PeriodYear == periodYear
                        && a.OrderId == orderId);

                if (specificAllocationId.HasValue)
                {
                    query = query.Where(a => a.DataCreditAllocationId == specificAllocationId.Value);
                }

                var allocationsToProcess = await query
                    .OrderBy(a => a.DataCreditAllocationId)
                    .ToListAsync();

                foreach (var allocation in allocationsToProcess)
                {
                    var parentPosition = await _context.TmpDataPositions
                        .Where(p => p.RowId == allocation.PositionId)
                        .Select(p => new
                        {
                            p.ParentRowId,
                            p.XRepType
                        })
                        .FirstOrDefaultAsync();

                    if (parentPosition?.ParentRowId != null
                        && allocation.AllocationTypeId != 277)
                    {
                        // Get parent employee and payplan
                        var parentInfo = await _context.TmpDataPositions
                            .Where(p => p.RowId == parentPosition.ParentRowId)
                            .Select(p => new
                            {
                                p.PrEmpId,
                                p.XRepType
                            })
                            .FirstOrDefaultAsync();

                        if (parentInfo != null)
                        {
                            // Create rollup allocation
                            var rollupAllocation = new DataCreditAllocation
                            {
                                AllocationTypeId = allocation.AllocationTypeId,
                                PositionId = parentPosition.ParentRowId,
                                OrderId = allocation.OrderId,
                                AllocationValue = allocation.AllocationValue,
                                ChildPositionId = allocation.PositionId,
                                EmployeeId = parentInfo.PrEmpId,
                                AllocationSource = "ROLLUP",
                                ItemId = allocation.ItemId,
                                AllocationList = allocation.AllocationList,
                                AdjustmentId = allocation.AdjustmentId,
                                Payplan = parentInfo.XRepType,
                                PeriodYear = periodYear,
                                PeriodMonth = periodMonth,
                                RollupProcessed = "N"
                            };

                            _context.DataCreditAllocations.Add(rollupAllocation);

                            var employeeName = await _context.DataEmployees
                                .Where(e => e.RowId == parentInfo.PrEmpId)
                                .Select(e => e.FstName + " " + e.LastName)
                                .FirstOrDefaultAsync();

                            _logger.LogDebug("Rollup for {Employee} of {Value}£ (allocId {AllocId})",
                                employeeName, allocation.AllocationValue, allocation.AllocationTypeId);

                            // FY 2025 specific logic - F1's rollup to J. and L. Dugdale
                            if (periodYear == "2025" && int.Parse(periodMonth) > 1)
                            {
                                if (parentPosition.ParentRowId == "1-4XJ12KT" || parentPosition.ParentRowId == "1-2Q1H7YC")
                                {
                                    _logger.LogDebug("Employee rollup to J. and L. Dugdale for employee {EmployeeId}", parentInfo.PrEmpId);

                                    // Rollup to Steph Reid (1-GI7ASH)
                                    var stephPosition = await _context.DataPositionHistories
                                        .Where(p => p.EmployeeRowId == "1-GI7ASH" && p.EndDt == null)
                                        .OrderByDescending(p => p.DateCreated)
                                        .Select(p => p.PositionRowId)
                                        .FirstOrDefaultAsync();

                                    if (stephPosition != null)
                                    {
                                        _context.DataCreditAllocations.Add(new DataCreditAllocation
                                        {
                                            AllocationTypeId = allocation.AllocationTypeId,
                                            PositionId = stephPosition,
                                            OrderId = allocation.OrderId,
                                            AllocationValue = allocation.AllocationValue,
                                            ChildPositionId = parentPosition.ParentRowId,
                                            EmployeeId = "1-GI7ASH",
                                            AllocationSource = "ROLLUP",
                                            ItemId = allocation.ItemId,
                                            AllocationList = allocation.AllocationList,
                                            AdjustmentId = allocation.AdjustmentId,
                                            Payplan = parentInfo.XRepType,
                                            PeriodYear = periodYear,
                                            PeriodMonth = periodMonth,
                                            RollupProcessed = "N"
                                        });
                                    }

                                    // Rollup to Mark Statton (1-2D0KXXD)
                                    var markPosition = await _context.DataPositionHistories
                                        .Where(p => p.EmployeeRowId == "1-2D0KXXD" && p.EndDt == null)
                                        .OrderByDescending(p => p.DateCreated)
                                        .Select(p => p.PositionRowId)
                                        .FirstOrDefaultAsync();

                                    if (markPosition != null)
                                    {
                                        _context.DataCreditAllocations.Add(new DataCreditAllocation
                                        {
                                            AllocationTypeId = allocation.AllocationTypeId,
                                            PositionId = markPosition,
                                            OrderId = allocation.OrderId,
                                            AllocationValue = allocation.AllocationValue,
                                            ChildPositionId = parentPosition.ParentRowId,
                                            EmployeeId = "1-2D0KXXD",
                                            AllocationSource = "ROLLUP",
                                            ItemId = allocation.ItemId,
                                            AllocationList = allocation.AllocationList,
                                            AdjustmentId = allocation.AdjustmentId,
                                            Payplan = parentInfo.XRepType,
                                            PeriodYear = periodYear,
                                            PeriodMonth = periodMonth,
                                            RollupProcessed = "N"
                                        });
                                    }
                                }
                            }
                            else // FY 2024 - F1's rollup to D. Milleage
                            {
                                if (parentPosition.ParentRowId == "1-ZZI9WJ")
                                {
                                    _logger.LogDebug("Employee rollup to D. Milleage for employee {EmployeeId}", parentInfo.PrEmpId);

                                    // Rollup to Steph Reid (1-GI7ASH)
                                    var stephPosition = await _context.DataPositionHistories
                                        .Where(p => p.EmployeeRowId == "1-GI7ASH" && p.EndDt == null)
                                        .OrderByDescending(p => p.DateCreated)
                                        .Select(p => p.PositionRowId)
                                        .FirstOrDefaultAsync();

                                    if (stephPosition != null)
                                    {
                                        _context.DataCreditAllocations.Add(new DataCreditAllocation
                                        {
                                            AllocationTypeId = allocation.AllocationTypeId,
                                            PositionId = stephPosition,
                                            OrderId = allocation.OrderId,
                                            AllocationValue = allocation.AllocationValue,
                                            ChildPositionId = parentPosition.ParentRowId,
                                            EmployeeId = "1-GI7ASH",
                                            AllocationSource = "ROLLUP",
                                            ItemId = allocation.ItemId,
                                            AllocationList = allocation.AllocationList,
                                            AdjustmentId = allocation.AdjustmentId,
                                            Payplan = parentInfo.XRepType,
                                            PeriodYear = periodYear,
                                            PeriodMonth = periodMonth,
                                            RollupProcessed = "N"
                                        });
                                    }

                                    // Rollup to Mark Statton (1-2D0KXXD)
                                    var markPosition = await _context.DataPositionHistories
                                        .Where(p => p.EmployeeRowId == "1-2D0KXXD" && p.EndDt == null)
                                        .OrderByDescending(p => p.DateCreated)
                                        .Select(p => p.PositionRowId)
                                        .FirstOrDefaultAsync();

                                    if (markPosition != null)
                                    {
                                        _context.DataCreditAllocations.Add(new DataCreditAllocation
                                        {
                                            AllocationTypeId = allocation.AllocationTypeId,
                                            PositionId = markPosition,
                                            OrderId = allocation.OrderId,
                                            AllocationValue = allocation.AllocationValue,
                                            ChildPositionId = parentPosition.ParentRowId,
                                            EmployeeId = "1-2D0KXXD",
                                            AllocationSource = "ROLLUP",
                                            ItemId = allocation.ItemId,
                                            AllocationList = allocation.AllocationList,
                                            AdjustmentId = allocation.AdjustmentId,
                                            Payplan = parentInfo.XRepType,
                                            PeriodYear = periodYear,
                                            PeriodMonth = periodMonth,
                                            RollupProcessed = "N"
                                        });
                                    }
                                }
                            }
                        }
                    }

                    // Mark allocation as processed
                    allocation.RollupProcessed = "Y";
                }

                await _context.SaveChangesAsync();

                // Remove entries for excluded positions
                var excludedPositions = await _context.ConfigSystemParameters
                    .Where(p => p.ParameterName == "REVENUE_EXCLUDED_POSITIONS")
                    .Select(p => p.ParameterValue)
                    .ToListAsync();

                if (excludedPositions.Any())
                {
                    var allocationsToDelete = await _context.DataCreditAllocations
                        .Where(a => excludedPositions.Contains(a.PositionId))
                        .ToListAsync();

                    _context.DataCreditAllocations.RemoveRange(allocationsToDelete);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in rollup allocation");
                throw;
            }
        }
    }
}