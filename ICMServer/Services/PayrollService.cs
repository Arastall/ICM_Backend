using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface IPayrollService
    {
        Task<List<PayrollWorkdayBonusResult>> GeneratePayrollWorkdayBonusAsync(string year, string month);
    }

    public class PayrollService : IPayrollService
    {
        private readonly IOrderPreparationService _orderPreparation;
        private readonly ICreditAllocationService _creditAllocation;
        private readonly ICommissionPaymentService _commissionPayment;
        private readonly ISystemParametersHelper _sysParams;
        private readonly IPeriodContext _periodContext;
        private readonly ILogger<PayrollService> _logger;
        private readonly IServiceProvider _sp;
        private readonly ICMDBContext _context;

        public PayrollService(
            IOrderPreparationService orderPreparation,
            ICreditAllocationService creditAllocation,
            ICommissionPaymentService commissionPayment,
            ISystemParametersHelper sysParams,
            IPeriodContext periodContext,
            ILogger<PayrollService> logger,
            IServiceProvider sp)
        {
            _orderPreparation = orderPreparation;
            _creditAllocation = creditAllocation;
            _commissionPayment = commissionPayment;
            _periodContext = periodContext;
            _sysParams = sysParams;
            _logger = logger;
            _sp = sp;

            var scope = _sp.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

        }

        public async Task<List<PayrollWorkdayBonusResult>> GeneratePayrollWorkdayBonusAsync(string year, string month)
        {
            _logger.LogInformation("Generating Payroll Workday Bonus for {Year}/{Month}", year, month);

            try
            {
                // 1. Récupérer les guarantees actives
                var guarantees = await GetActiveGuaranteesAsync();

                // 2. Récupérer les paiements de commission
                var payments = await GetCommissionPaymentsAsync(year, month);

                // 3. Appliquer la logique métier
                var result = payments
                    .GroupBy(p => new
                    {
                        p.EmployeeId,
                        p.EmployeeNumber,
                        p.FirstName,
                        p.LastName
                    })
                    .Select(g => BuildPayrollRecord(g, guarantees))
                    .OrderBy(r => r.LastName)
                    .ThenBy(r => r.FirstName)
                    .ToList();

                _logger.LogInformation("Generated {Count} payroll records", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating payroll workday bonus");
                throw;
            }
        }

        // Requête pure : récupérer les guarantees
        private async Task<Dictionary<string, decimal>> GetActiveGuaranteesAsync()
        {
            var cutoffDate = DateTime.Now;

            var guarantees = await _context.DataNewcomerSettings
                .Where(ns => ns.Guarantee != null && ns.Guarantee > 0)
                .Where(ns => EF.Functions.DateDiffMonth(ns.PeriodStartDate, cutoffDate) <= ns.PeriodDurationInMonths)
                .ToDictionaryAsync(
                    ns => ns.EmployeeRowId,
                    ns => ns.Guarantee.Value
                );

            _logger.LogDebug("Found {Count} active guarantees", guarantees.Count);

            return guarantees;
        }

        // Requête pure : récupérer les paiements
        private async Task<List<CommissionPaymentData>> GetCommissionPaymentsAsync(string year, string month)
        {
            var payments = await _context.DataCommissionPayments
                .Where(p => p.PeriodYear == year)
                .Where(p => p.PeriodMonth == month)
                .Where(p => p.PaymentWitheld == false)
                .Join(
                    _context.DataEmployees,
                    p => p.EmployeeId,
                    e => e.RowId,
                    (p, e) => new { Payment = p, Employee = e }
                )
                .Where(x => x.Employee.EmployeeNumber != "0-1" || x.Employee.EmployeeNumber == null)
                .Select(x => new CommissionPaymentData
                {
                    EmployeeId = x.Employee.RowId,
                    EmployeeNumber = x.Employee.EmployeeNumber,
                    FirstName = x.Employee.FstName,
                    LastName = x.Employee.LastName,
                    PaymentSource = x.Payment.PaymentSource,
                    PaymentValue = x.Payment.PaymentValue ?? 0
                })
                .ToListAsync();

            _logger.LogDebug("Retrieved {Count} commission payments", payments.Count);

            return payments;
        }

        // Logique métier : construire un enregistrement de paie
        private PayrollWorkdayBonusResult BuildPayrollRecord(IGrouping<dynamic, CommissionPaymentData> group, Dictionary<string, decimal> guarantees)
        {
            var employeeId = group.Key.EmployeeId;

            var commissionSum = group
                .Where(p => p.PaymentSource != "BONUS")
                .Sum(p => p.PaymentValue);

            var bonusSum = group
                .Where(p => p.PaymentSource == "BONUS")
                .Sum(p => p.PaymentValue);

            // Appliquer la garantie si elle existe
            var finalCommission = commissionSum;
            if (guarantees.ContainsKey(employeeId))
            {
                var guarantee = guarantees[employeeId];
                finalCommission = Math.Max(commissionSum, guarantee);

                if (finalCommission > commissionSum)
                {
                    _logger.LogDebug($"Applied guarantee for {employeeId}: {commissionSum} -> {finalCommission}");
                }
            }

            return new PayrollWorkdayBonusResult
            {
                FirstName = group.Key.FirstName,
                LastName = group.Key.LastName,
                NewEmployeeCode = MapEmployeeCode(group.Key.EmployeeNumber),
                Commission = Math.Round(finalCommission, 2),
                Bonus = Math.Round(bonusSum, 2)
            };
        }

        // Logique métier : mapper le code employé
        private string MapEmployeeCode(string employeeNumber)
        {
            // Cas spécial hardcodé
            if (employeeNumber == "00702127")
                return "10005357";

            if (string.IsNullOrEmpty(employeeNumber))
                return "NO EMPLOYEE NUMBER";

            // Vérifier si c'est un nombre valide
            if (!long.TryParse(employeeNumber, out long number))
                return "NO EMPLOYEE NUMBER";

            // Nouveau format Workday (>= 10000000)
            if (number >= 10000000)
                return employeeNumber;

            // Ancien format qui devrait être dans la lookup table
            return "MISSING FROM LOOKUP";
        }
    }

    // DTO pour transporter les données
    public class CommissionPaymentData
    {
        public string EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PaymentSource { get; set; }
        public decimal PaymentValue { get; set; }
    }

    // Classe résultat
    public class PayrollWorkdayBonusResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NewEmployeeCode { get; set; }
        public decimal Commission { get; set; }
        public decimal Bonus { get; set; }
    }
}