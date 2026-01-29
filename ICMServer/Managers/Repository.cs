using ClosedXML.Excel;
using ICMServer.Classes;
using ICMServer.DBContext;
using ICMServer.Interfaces;
using ICMServer.Models;
using ICMServer.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ICMServer.Managers
{
    public class Repository : IRepository
    {
        private readonly ILogger<IRepository> _logger;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IServiceProvider _serviceProvider;
        private readonly IImportService _importService;
        private readonly ICalculationService _calculationService;
        private readonly IPeriodContext _periodContext;
        private readonly IProcessStateService _processState;
        private readonly string _currentPeriodYear;
        private readonly string _currentPeriodMonth;

        private readonly Dictionary<string, string> _dicoMonths = new Dictionary<string, string>
            {
                { "01", "Feb" }, { "02", "Mar" }, { "03", "Apr" }, { "04", "May" },
                { "05", "Jun" }, { "06", "Jul" }, { "07", "Aug" }, { "08", "Sep" },
                { "09", "Oct" }, { "10", "Nov" }, { "11", "Dec" }, { "12", "Jan" }
            };

        private readonly string[] headersRevenues = { "Reference ID", "Sale Period", "Revenue", "Original Order Date", "Employee", "Reason", "Customer name", "Account Number", "Order Number", "Adj. Order Number", "Product desc.", "Allocation Source", "Position", "Pay plan", "Allocation", "Allocation ID" };
        private readonly string[] headersPayments = { "Sales Period", "Employee", "Comm. ID", "Account Number", "Customer Name", "Order Number", "Adj. Order Number", "Payment Description", "Source", "Rate", "Payment" };
        private readonly string[] headersEmployees = { "Order Number", "Year", "Month", "Employee ID", "Employee Name", "Alloc. Percentage" };
        private readonly string[] headersHeaders = { "DATA_ORDER_HEADERS_ID", "ORDER_ROW_ID", "ORDER_NUMBER", "SAP_ORDER_REFERENCE", "ORDER_TYPE", "PROMOTION_CODE", "CUSTOMER_ACCOUNT_NUMBER", "CUSTOMER_NAME", "CUSTOMER_TYPE", "PR_POSTN_ID", "ORDER_LIST_VAL", "ORDER_SALE_VAL", "ORDER_DISCOUNT_VAL", "ORDER_DISCOUNT_PERCENT", "SERVICE_LIST_VAL", "SERVICE_SALE_VAL", "SERVICE_DISCOUNT_VAL", "SERVICE_DISCOUNT_PERCENT", "SERVICE_TYPE", "DATE_CREATED", "PERIOD_MONTH", "PERIOD_YEAR", "MAINTENANCE_TERM" };
        private readonly string[] headersItems = { "ORDER_NUMBER", "ORDER_ITEM_ID", "ORDER_ROW_ID", "LINE_NUMBER", "PRODUCT_CODE", "PRODUCT_DESC", "LIST_VALUE", "SALE_VALUE", "QUANTITY", "PRODUCT_LEVEL_1", "PRODUCT_LEVEL_2", "PRODUCT_LEVEL_3", "PRODUCT_TYPE", "DATE_CREATED" };

        public Repository(ILogger<IRepository> logger,
                              IServiceProvider sp,
                              IImportService importService,
                              ICalculationService calculationService,
                              IPeriodContext periodContext,
                              IHubContext<NotificationHub> hub,
                              IProcessStateService processState)
        {
            _logger = logger;
            _serviceProvider = sp;
            _importService = importService;
            _calculationService = calculationService;
            _periodContext = periodContext;
            _hub = hub;
            _processState = processState;

            _currentPeriodMonth = _periodContext.GetPeriodMonth();
            _currentPeriodYear = _periodContext.GetPeriodYear();
        }

        public List<Menu> GetMenuList()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            var menus = db.ConfigIcmAdminScreens.Where(s => s.GroupId == 1).ToList();
            return db.ConfigIcmAdminScreens.Where(s => s.GroupId == 1).ToList().Select(a => new Menu() { ScreenName = a.ScreenName, ScreenTitle = a.ScreenTitle }).ToList();
        }

        public async Task RunICM()
        {
            // Initialize process state with steps
            _processState.StartProcess(new List<ProcessStep>
            {
                new() { StepId = "import_siebel_data", StepName = "Importing Siebel Data", Status = "pending" },
                new() { StepId = "calculating", StepName = "Processing Calculations", Status = "pending" }
            });

            try
            {
                await Task.Delay(2000);

                var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var data = context.ConfigSystemParameters.FirstOrDefault(p => p.ParameterName == "RUN_ID");
                if (data != null)
                {
                    data.ParameterValue = (int.Parse(data.ParameterValue ?? "0") + 1).ToString();
                    await context.SaveChangesAsync();
                }

                context.RunIcmInfos.Add(new Models.RunIcmInfo { BeginDate = DateTime.Now, Status = "Running" });
                await context.SaveChangesAsync();

                // Notify clients that process has started - get last completed run for display
                var lastCompletedRun = context.RunIcmInfos
                    .Where(r => r.Status == "Completed" && r.EndDate != null)
                    .OrderByDescending(r => r.RunIcmInfoId)
                    .FirstOrDefault();

                await _hub.Clients.All.SendAsync("ServerStatusUpdate", new
                {
                    status = "Running",
                    dtLastRun = lastCompletedRun?.EndDate,
                    time = DateTime.Now
                });

                await _importService.DoImportAsync();
                await _calculationService.DoCalculationsAsync();

                var endTime = DateTime.Now;
                context.RunIcmInfos.Add(new Models.RunIcmInfo { EndDate = endTime, Status = "Completed" });
                await context.SaveChangesAsync();

                _processState.FinishProcess(true, "ICM calculation completed successfully");

                await _hub.Clients.All.SendAsync("ProcessFinished", new
                {
                    success = true,
                    message = "ICM calculation completed successfully",
                    time = endTime
                });

                // Notify clients to update header status with new last run date
                await _hub.Clients.All.SendAsync("ServerStatusUpdate", new
                {
                    status = "Completed",
                    dtLastRun = endTime,
                    time = endTime
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during RunICM");
                _processState.FinishProcess(false, $"ICM calculation failed: {ex.Message}");

                await _hub.Clients.All.SendAsync("ProcessFinished", new
                {
                    success = false,
                    message = $"ICM calculation failed: {ex.Message}",
                    time = DateTime.Now
                });
                throw;
            }
        }

        public async Task RunICMOld()
        {
            // Initialize process state with steps for old calculation
            _processState.StartProcess(new List<ProcessStep>
            {
                new() { StepId = "import_siebel_data", StepName = "Importing Siebel Data", Status = "pending" },
                new() { StepId = "do_calculation_old", StepName = "Calculating Figures (Legacy)", Status = "pending" }
            });

            try
            {
                await Task.Delay(2000);

                var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                // Mark process as running
                context.RunIcmInfos.Add(new Models.RunIcmInfo { BeginDate = DateTime.Now, Status = "Running" });
                await context.SaveChangesAsync();

                // Notify clients that process has started
                var lastCompletedRun = context.RunIcmInfos
                    .Where(r => r.Status == "Completed" && r.EndDate != null)
                    .OrderByDescending(r => r.RunIcmInfoId)
                    .FirstOrDefault();

                await _hub.Clients.All.SendAsync("ServerStatusUpdate", new
                {
                    status = "Running",
                    dtLastRun = lastCompletedRun?.EndDate,
                    time = DateTime.Now
                });

                await _importService.DoImportAsync();

                await _calculationService.DoCalculationsOldAsync();

                var endTime = DateTime.Now;
                context.RunIcmInfos.Add(new Models.RunIcmInfo { EndDate = endTime, Status = "Completed" });
                await context.SaveChangesAsync();

                _processState.FinishProcess(true, "ICM calculation completed successfully");

                await _hub.Clients.All.SendAsync("ProcessFinished", new
                {
                    success = true,
                    message = "ICM calculation completed successfully",
                    time = endTime
                });

                // Notify clients to update header status with new last run date
                await _hub.Clients.All.SendAsync("ServerStatusUpdate", new
                {
                    status = "Completed",
                    dtLastRun = endTime,
                    time = endTime
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during RunICMOld");
                _processState.FinishProcess(false, $"ICM calculation failed: {ex.Message}");

                await _hub.Clients.All.SendAsync("ProcessFinished", new
                {
                    success = false,
                    message = $"ICM calculation failed: {ex.Message}",
                    time = DateTime.Now
                });
                throw;
            }
        }

        public Task RunICMFrontend()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            db.Database.ExecuteSqlRawAsync("EXEC usp_RUN_ICM");
            return Task.CompletedTask;
            //using var scope = _serviceProvider.CreateScope();
            //var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            //var forceICM = db.ConfigSystemParameters.FirstOrDefault(a => a.ParameterName == "FORCE_ICM");

            //if (forceICM != null)
            //{
            //    if (forceICM.ParameterValue == "1")
            //    {
            //        _logger.Log(LogLevel.Warning, "Run ICM called from Website, but ICM is already running !");
            //        return false;
            //    }
            //    else
            //    {
            //        forceICM.ParameterValue = "1";
            //        var result = db.SaveChanges();

            //        _logger.Log(LogLevel.Information, "Run ICM called from Website. Agent will launch it soon.");
            //        return true;
            //    }
            //}
            //else
            //{
            //    _logger.Log(LogLevel.Information, "Parameter FORCE_ICM doesn't exist. We create it.");

            //    forceICM = new ConfigSystemParameter
            //    {
            //        ParameterName = "FORCE_ICM",
            //        ParameterValue = "1"
            //    };

            //    db.ConfigSystemParameters.Add(forceICM);
            //    db.SaveChanges();

            //    _logger.Log(LogLevel.Information, "[SUCCESS] Agent will launch it soon.");
            //    return true;
            //}
        }

        public MainInfo GetMainInfo()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var lastRun = db.RunIcmInfos
                    .OrderByDescending(a => a.RunIcmInfoId)
                    .FirstOrDefault();

                var lastRunId = db.ConfigSystemParameters.FirstOrDefault(p => p.ParameterName == "RUN_ID").ParameterValue;

                if (lastRun == null)
                {
                    return new MainInfo
                    {
                        Status = "NO_DATA",
                        BeginDate = null,
                        EndDate = null,
                        User = "NEOPOSTAD\\FRCW000887",
                        Salesperiod = $"FY{_currentPeriodYear} SP{_currentPeriodMonth:D2}",
                        LastRunId = Convert.ToInt32(lastRunId)
                    };
                }

                return new MainInfo
                {
                    Status = lastRun.Status ?? "UNKNOWN",
                    BeginDate = lastRun.BeginDate,
                    EndDate = lastRun.EndDate,
                    User = "NEOPOSTAD\\FRCW000887", // ✅ Valeur fixe si pas de colonne User
                    Salesperiod = $"FY{_currentPeriodYear} SP{_currentPeriodMonth:D2}",
                    LastRunId = Convert.ToInt32(lastRunId)
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting MainInfo");

                return new MainInfo
                {
                    Status = "ERROR",
                    BeginDate = null,
                    EndDate = null,
                    User = "System",
                    Salesperiod = $"FY{_currentPeriodYear} SP{_currentPeriodMonth:D2}",
                };
            }
        }

        public object GetLastRunInfo()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            // Get the most recent run
            var lastRun = db.RunIcmInfos.OrderByDescending(r => r.RunIcmInfoId).FirstOrDefault();

            if (lastRun == null)
            {
                return new { status = "Never", dtLastRun = (DateTime?)null };
            }

            // If status is "Running", find the last completed run for dtLastRun
            if (lastRun.Status == "Running")
            {
                var lastCompletedRun = db.RunIcmInfos
                    .Where(r => r.Status == "Completed" && r.EndDate != null)
                    .OrderByDescending(r => r.RunIcmInfoId)
                    .FirstOrDefault();

                return new
                {
                    status = "Running",
                    dtLastRun = lastCompletedRun?.EndDate
                };
            }

            return new { status = lastRun.Status, dtLastRun = lastRun.EndDate };
        }


        public DataEmployee GetEmployee(string empId)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            return db.DataEmployees.FirstOrDefault(p => p.RowId == empId);
        }

        public List<DataEmployee> SearchEmployee(string value)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            return db.DataEmployees.Where(e => !e.LastName.ToLower().Contains("left") && (e.FstName.ToLower().StartsWith(value.ToLower()) || e.LastName.ToLower().StartsWith(value.ToLower()))).ToList();
        }

        public DataEmployee SearchEmployeeBySurname(string surname)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            return db.DataEmployees.FirstOrDefault(p => p.LastName == surname);
        }


        public List<AchievementHistory> GetAchievementHistory(string employeeId)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            employeeId = "1-2YE12TM";
            // 1. Récupérer tous les OrderId où l’employé est impliqué via crédit (distinct)
            var employeeOrderIds = db.DataCreditAllocations
                .Where(c => c.EmployeeId == employeeId && c.PeriodYear == "2025" && c.AllocationTypeId == 264)
                .Select(c => c.OrderId)
                .Distinct()
                .ToList();

            // 2. Pour chaque OrderId, récupérer la dernière date de statut (invoicing date)
            var dealsWithStatusDates = new List<(string OrderId, DateTime? StatusDate, decimal TotalAchievement)>();

            foreach (var orderId in employeeOrderIds)
            {
                if (!string.IsNullOrWhiteSpace(orderId) && orderId != "NOT_FOUND")
                {
                    var lastStatusDate = db.DataOrderStatusHistories
                        .Where(s => s.RowId == orderId)
                        .OrderByDescending(s => s.StatusDt)
                        .Select(s => (DateTime?)s.StatusDt)
                        .FirstOrDefault();

                    dealsWithStatusDates.Add((orderId, lastStatusDate, 0));
                }
            }


            // 3. Trier la liste par StatusDate (InvoicingDate)
            var orderedDeals = dealsWithStatusDates
                .OrderBy(d => d.StatusDate)
                .ToList();


            // ✅ Résultat final dans orderedDeals :
            // List<(string OrderId, DateTime? StatusDate)>
            var history = new List<AchievementHistory>();
            var totalRevenues = decimal.Zero;
            var revenuesOrder = decimal.Zero;
            var rateToApply = decimal.Zero;
            var totalPercentage = decimal.Zero;

            var target = db.DataDefaultTargets.FirstOrDefault(t => t.FinYear == "2025" && t.PayPlanType == "PM").TargetValue.Value;
            for (var i = 0; i < orderedDeals.Count; i++)
            {
                var order = orderedDeals[i];
                revenuesOrder = db.DataCreditAllocations.Where(r => r.EmployeeId == employeeId && r.OrderId == orderedDeals[i].OrderId && r.AllocationTypeId == 264).Sum(r => r.AllocationValue).Value;
                totalRevenues += revenuesOrder;
                totalPercentage = (totalRevenues / target) * 100;

                var rateData = db.DataPerformanceAgainstTargetMatrices.FirstOrDefault(
                    r =>
                    r.Payplan == "FM" &&
                    r.EmployeeId == "X" &&
                    r.AllocationId == 264 &&
                    r.PeriodYear == 2025 &&
                    r.PeriodMonthStart <= 5 &&
                    r.PeriodMonthEnd >= 5 &&
                    r.TargetPercentageStart <= totalPercentage &&
                    r.TargetPercentageEnd > totalPercentage);

                if (rateData != null && rateData.CommissionValue != null)
                {
                    rateToApply = rateData.CommissionValue.Value;
                }

                var ach = new AchievementHistory { OrderId = order.OrderId, AchievementDate = order.StatusDate.Value, TotalOrder = revenuesOrder, TotalAchievement = totalRevenues, PercentageAchievement = totalPercentage, RateToApply = rateToApply };
                history.Add(ach);
            }

            return history;
        }


        public object GetCurrentSalesPeriod()
        {
            var month = _periodContext.GetPeriodMonth();
            var year = _periodContext.GetPeriodYear();
            return new { Month = month, Year = year };
        }

        public string GetCurrentSalesPeriodStr()
        {
            var month = _periodContext.GetPeriodMonth();
            var year = _periodContext.GetPeriodYear();
            return $"{_dicoMonths[month]} {year}";
        }

        public bool SetCurrentSalesPeriod(string year, string month)
        {
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                return false;
            }

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            var monthObj = db.ConfigSystemParameters.FirstOrDefault(c => c.ParameterName == "PERIOD_MONTH");
            var yearObj = db.ConfigSystemParameters.FirstOrDefault(c => c.ParameterName == "PERIOD_YEAR");

            if (monthObj != null && yearObj != null)
            {
                monthObj.ParameterValue = month;
                yearObj.ParameterValue = year;

                db.ConfigSystemParameters.Update(monthObj);
                db.ConfigSystemParameters.Update(yearObj);

                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public byte[] GetYearlyRevenuesPaymentsReport(string year)
        {


            // Génération du fichier Excel en mémoire
            using (var workbook = new XLWorkbook())
            {

                GetRevenuesSheet(workbook, year);

                GetPaymentsSheet(workbook, year);


                // Conversion en Base64
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }

            }
        }

        public byte[] GetMonthRevenuesPaymentsReport(string year, string month)
        {

            // Génération du fichier Excel en mémoire
            using (var workbook = new XLWorkbook())
            {

                GetRevenuesSheet(workbook, year, month);

                GetPaymentsSheet(workbook, year, month);


                // Conversion en Base64
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }

            }
        }

        public byte[] GetMonthlyDealsInfoReport(string year, string month)
        {

            // Génération du fichier Excel en mémoire
            using (var workbook = new XLWorkbook())
            {
                GetEmployeesSheet(workbook, year, month);

                GetHeadersSheet(workbook, year, month);

                GetItemsSheet(workbook, year, month);


                // Conversion en Base64
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }

            }
        }

        public byte[] GetPayFileReport(string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            // Génération du fichier Excel en mémoire
            using (var workbook = new XLWorkbook())
            {
                GetSPPayFileFigures(workbook, year, month);

                // Conversi;on en Base64
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] GetPayFileReportOld(string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            // Génération du fichier Excel en mémoire
            using (var workbook = new XLWorkbook())
            {
                GetSPPayFileFiguresOld(workbook, year, month);

                // Conversi;on en Base64
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] GetOrderSequenceFromLogs(int runID)
        {
            // Génération du fichier Excel en mémoire
            using (var workbook = new XLWorkbook())
            {
                GetOrderSequence(workbook, runID);

                // Conversi;on en Base64
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private void GetOrderSequence(XLWorkbook workbook, int runID)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            var worksheet = workbook.Worksheets.Add($"Order Sequence ${runID}");

            SetOrderSequenceHeaders(worksheet);

            var query = db.SystemLogs
                .Where(p => p.LogCountId.Value == runID
                         && p.LogSource == "usp_DO_CALCULATIONS"
                         && p.LogDesc.StartsWith("Order "))
                .OrderBy(p => p.SystemLogId)
                .AsEnumerable() // <-- exécute la requête SQL, le reste se fait en mémoire
                .Select(p =>
                {
                    var orderParts = p.LogDesc.Replace("Order ", "").Split('/');
                    var reference = p.ReferenceValue ?? "";
                    var refParts = reference.Split(' ');

                    var orderNumber = refParts.Length > 0 ? refParts[0] : "";

                    var orderId = "";
                    var start = reference.IndexOf('(');
                    var end = reference.IndexOf(')');
                    if (start >= 0 && end > start)
                        orderId = reference.Substring(start + 1, end - start - 1);

                    var invoicingDate = "";
                    var marker = "invoiced on the ";
                    var pos = reference.IndexOf(marker);
                    if (pos >= 0)
                        invoicingDate = reference.Substring(pos + marker.Length).Trim();

                    return new
                    {
                        Index = orderParts.Length > 0 ? orderParts[0].Trim() : "",
                        Total = orderParts.Length > 1 ? orderParts[1].Trim() : "",
                        OrderNumber = orderNumber,
                        OrderId = orderId,
                        InvoicingDate = invoicingDate
                    };
                })
                .ToList();


            int row = 2;

            foreach (var item in query)
            {
                worksheet.Cell(row, 1).Value = item.OrderNumber;
                worksheet.Cell(row, 2).Value = item.OrderId;
                worksheet.Cell(row, 3).Value = item.InvoicingDate;
                worksheet.Cell(row, 4).Value = item.Index;
                worksheet.Cell(row, 5).Value = item.Total;
                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, query.Count + 1, 3);
            dataRange.SetAutoFilter();
        }

        private void GetSPPayFileFigures(XLWorkbook workbook, string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            var worksheet = workbook.Worksheets.Add("Pay File");

            SetPayFileHeaders(worksheet);
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Collecting data ...", time = DateTime.Now });

            var query =
                   from p in db.DataCommissionPayments
                   join e in db.DataEmployees on p.EmployeeId equals e.RowId
                   where e.DeleteFlag == 0
                         && p.PeriodYear == year
                         && p.PeriodMonth == month
                         && !p.PaymentWitheld
                         && (e.EmployeeNumber != "0-1" || e.EmployeeNumber == null)
                   group p by new
                   {
                       EmployeeName = e.FstName + " " + e.LastName,
                       PayElement = p.PaymentSource == "BONUS" ? "Bonus" : "Commission"
                   }
                   into g
                   orderby g.Key.EmployeeName, g.Key.PayElement
                   select new
                   {
                       EmployeeName = g.Key.EmployeeName,
                       PayrollPayElementName = g.Key.PayElement,
                       Amount = Math.Round(g.Sum(x => (decimal?)(x.PaymentValue ?? 0)) ?? 0, 2)
                   };

            var result = query.ToList();

            int row = 2;
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Creating xlsx file ...", time = DateTime.Now });
            foreach (var item in result)
            {
                worksheet.Cell(row, 1).Value = item.EmployeeName;
                worksheet.Cell(row, 2).Value = item.PayrollPayElementName;
                worksheet.Cell(row, 3).Value = item.Amount;
                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, result.Count + 1, 3);
            dataRange.SetAutoFilter();
        }

        private void SetPayFileHeaders(IXLWorksheet worksheet)
        {
            // En-têtes exacts
            string[] headers = new[]
            {
                "Employee Name",
                "Payroll Pay Element Name",
                "Amount"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
        }

        private async void GetSPPayFileFiguresOld(XLWorkbook workbook, string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            var worksheet = workbook.Worksheets.Add("Pay File");

            SetPayFileHeadersOld(worksheet);
            await _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Collecting data PayFile ...", time = DateTime.Now });

            var guarantees = (from ns in db.DataNewcomerSettings
                              where ns.PeriodStartDate >= DateTime.Now.AddMonths(-1 * ns.PeriodDurationInMonths.Value)
                                    && ns.Guarantee != null
                              select new
                              {
                                  EmpId = ns.EmployeeRowId,
                                  Guarantee = ns.Guarantee
                              }).ToList();

            var guaranteesDict = guarantees.ToDictionary(g => g.EmpId, g => g.Guarantee);

            // 2. Requête SQL simple sans le join guarantees
            var rawData = await (from dcp in db.DataCommissionPayments
                                 join de in db.DataEmployees on dcp.EmployeeId equals de.RowId into deGroup
                                 from de in deGroup.DefaultIfEmpty()
                                 where dcp.PeriodMonth == month
                                       && dcp.PeriodYear == year
                                       && dcp.PaymentWitheld == false
                                       && (de.EmployeeNumber != "0-1" || de.EmployeeNumber == null)
                                 select new
                                 {
                                     dcp.PaymentSource,
                                     dcp.PaymentValue,
                                     EmployeeRowId = de.RowId,
                                     de.EmployeeNumber,
                                     de.FstName,
                                     de.LastName
                                 }).ToListAsync();

            // 3. Faire le grouping, le join avec guarantees et les calculs côté client
            var result = rawData
                .GroupBy(x => new
                {
                    x.EmployeeRowId,
                    x.EmployeeNumber,
                    x.FstName,
                    x.LastName
                })
                .Select(g =>
                {
                    // Récupérer la garantie depuis le dictionnaire
                    decimal? guarantee = guaranteesDict.ContainsKey(g.Key.EmployeeRowId)
                        ? guaranteesDict[g.Key.EmployeeRowId]
                        : null;

                    var commissionSum = g.Sum(x => x.PaymentSource == "BONUS" ? 0 : (x.PaymentValue ?? 0));
                    var bonusSum = g.Sum(x => x.PaymentSource == "BONUS" ? (x.PaymentValue ?? 0) : 0);

                    return new PayrollWorkdayBonusResult
                    {
                        FirstName = g.Key.FstName,
                        LastName = g.Key.LastName,
                        NewEmployeeCode = GetEmployeeCode(g.Key.EmployeeNumber),
                        Commission = guarantee.HasValue
                            ? (commissionSum < guarantee.Value ? guarantee.Value : commissionSum)
                            : commissionSum,
                        Bonus = bonusSum
                    };
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList();

            int row = 2;
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Creating xlsx PayFile ...", time = DateTime.Now });
            foreach (var item in result)
            {
                worksheet.Cell(row, 1).Value = item.FirstName;
                worksheet.Cell(row, 2).Value = item.LastName;
                worksheet.Cell(row, 3).Value = item.NewEmployeeCode;
                worksheet.Cell(row, 4).Value = item.Commission;
                worksheet.Cell(row, 5).Value = item.Bonus;
                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, result.Count + 1, 3);
            dataRange.SetAutoFilter();
        }

        private string GetEmployeeCode(string employeeNumber)
        {
            if (employeeNumber == "00702127")
                return "10005357";

            if (string.IsNullOrEmpty(employeeNumber))
                return "NO EMPLOYEE NUMBER";

            if (long.TryParse(employeeNumber, out long empNum))
            {
                return empNum >= 10000000 ? employeeNumber : "MISSING FROM LOOKUP";
            }

            return "NO EMPLOYEE NUMBER";
        }

        private void SetPayFileHeadersOld(IXLWorksheet worksheet)
        {
            // En-têtes exacts
            string[] headers = new[]
            {
                "Firstname",
                "Lastname",
                "New Employee_Code",
                "COMMISSION - Commission/Amount",
                "BONUS - Bonus/Amount"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
        }

        // Classe pour le résultat
        public class PayrollWorkdayBonusResult
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NewEmployeeCode { get; set; }
            public decimal Commission { get; set; }
            public decimal Bonus { get; set; }
        }


        public async void InsertAdjustmentsAsync(List<DataManualAdjustment> adjustments)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            _logger.LogInformation($"#### {adjustments.Count} adjustment(s) to insert.");

            await db.DataManualAdjustments.AddRangeAsync(adjustments);
            await db.SaveChangesAsync();

            _logger.LogInformation($"#### {adjustments.Count} adjustment(s) inserted !.");
        }







        private void SetOrderSequenceHeaders(IXLWorksheet worksheet)
        {
            // En-têtes exacts
            string[] headers = new[]
            {
                "Order Number",
                "Order ID",
                "Invoicing Date",
                "Index",
                "Total"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
        }

        private void GetEmployeesSheet(XLWorkbook workbook, string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = "Deals reps ...", time = DateTime.Now });

            var worksheet = workbook.Worksheets.Add("Employees");
            SetHeaders(worksheet, headersEmployees);

            var employeesFromDB = db.DataOrderPositions
                .Join(db.DataOrderHeaders, op => op.OrderRowId, oh => oh.OrderRowId, (op, oh) => new { op, oh })
                .Join(db.DataEmployees, x => x.op.EmployeeRowId, de => de.RowId, (x, de) => new { x.op, x.oh, de })
                .Where(x => x.oh.PeriodMonth == month && x.oh.PeriodYear == year)
                .Select(x => new
                {
                    x.oh.OrderNumber,
                    x.oh.PeriodYear,
                    x.oh.PeriodMonth,
                    x.op.EmployeeRowId,
                    Employee = x.de.FstName + " " + x.de.LastName,
                    x.op.AllocationPercentage
                })
                .ToList();

            int row = 2;

            foreach (var item in employeesFromDB)
            {
                worksheet.Cell(row, 1).Value = item.OrderNumber;
                worksheet.Cell(row, 2).Value = item.PeriodYear;
                worksheet.Cell(row, 3).Value = item.PeriodMonth;
                worksheet.Cell(row, 4).Value = item.EmployeeRowId;
                worksheet.Cell(row, 5).Value = item.Employee;
                worksheet.Cell(row, 6).Value = item.AllocationPercentage;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, employeesFromDB.ToList().Count + 1, 16);
            dataRange.SetAutoFilter();

        }

        private void GetHeadersSheet(XLWorkbook workbook, string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = "Deals headers ...", time = DateTime.Now });

            var worksheet = workbook.Worksheets.Add("Headers");
            SetHeaders(worksheet, headersHeaders);

            var headersFromDB = db.DataOrderHeaders
                .Where(h => h.PeriodMonth == month && h.PeriodYear == year)
                .Select(h => new
                {
                    h.DataOrderHeadersId,
                    h.OrderRowId,
                    h.OrderNumber,
                    h.SapOrderReference,
                    h.OrderType,
                    h.PromotionCode,
                    h.CustomerAccountNumber,
                    h.CustomerName,
                    h.CustomerType,
                    h.PrPostnId,
                    h.OrderListVal,
                    h.OrderSaleVal,
                    h.OrderDiscountVal,
                    h.OrderDiscountPercent,
                    h.ServiceListVal,
                    h.ServiceSaleVal,
                    h.ServiceDiscountVal,
                    h.ServiceDiscountPercent,
                    h.ServiceType,
                    h.DateCreated,
                    h.PeriodMonth,
                    h.PeriodYear,
                    h.MaintenanceTerm
                })
                .ToList();

            int row = 2;

            foreach (var item in headersFromDB)
            {
                worksheet.Cell(row, 1).Value = item.DataOrderHeadersId;
                worksheet.Cell(row, 2).Value = item.OrderRowId;
                worksheet.Cell(row, 3).Value = item.OrderNumber;
                worksheet.Cell(row, 4).Value = item.SapOrderReference;
                worksheet.Cell(row, 5).Value = item.OrderType;
                worksheet.Cell(row, 6).Value = item.PromotionCode;
                worksheet.Cell(row, 7).Value = item.CustomerAccountNumber;
                worksheet.Cell(row, 8).Value = item.CustomerName;
                worksheet.Cell(row, 9).Value = item.CustomerType;
                worksheet.Cell(row, 10).Value = item.PrPostnId;
                worksheet.Cell(row, 11).Value = item.OrderListVal;
                worksheet.Cell(row, 12).Value = item.OrderSaleVal;
                worksheet.Cell(row, 13).Value = item.OrderDiscountVal;
                worksheet.Cell(row, 14).Value = item.OrderDiscountPercent;
                worksheet.Cell(row, 15).Value = item.ServiceListVal;
                worksheet.Cell(row, 16).Value = item.ServiceSaleVal;
                worksheet.Cell(row, 17).Value = item.ServiceDiscountVal;
                worksheet.Cell(row, 18).Value = item.ServiceDiscountPercent;
                worksheet.Cell(row, 19).Value = item.ServiceType;
                worksheet.Cell(row, 20).Value = item.DateCreated;
                worksheet.Cell(row, 21).Value = item.PeriodMonth;
                worksheet.Cell(row, 22).Value = item.PeriodYear;
                worksheet.Cell(row, 23).Value = item.MaintenanceTerm;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, headersFromDB.Count + 1, 23);
            dataRange.SetAutoFilter();
        }

        private void GetItemsSheet(XLWorkbook workbook, string year, string month)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = "Deals items ...", time = DateTime.Now });

            var worksheet = workbook.Worksheets.Add("Items");
            SetHeaders(worksheet, headersItems);

            var itemsFromDB = db.DataOrderItems
                .Join(db.DataOrderHeaders, oi => oi.OrderRowId, oh => oh.OrderRowId, (oi, oh) => new { oi, oh })
                .Where(x => x.oh.PeriodMonth == month && x.oh.PeriodYear == year)
                .Select(x => new
                {
                    x.oh.OrderNumber,
                    x.oi.OrderItemId,
                    x.oi.OrderRowId,
                    x.oi.LineNumber,
                    x.oi.ProductCode,
                    x.oi.ProductDesc,
                    x.oi.ListValue,
                    x.oi.SaleValue,
                    x.oi.Quantity,
                    x.oi.ProductLevel1,
                    x.oi.ProductLevel2,
                    x.oi.ProductLevel3,
                    x.oi.ProductType,
                    x.oi.DateCreated
                })
                .ToList();

            int row = 2;

            foreach (var item in itemsFromDB)
            {
                worksheet.Cell(row, 1).Value = item.OrderNumber;
                worksheet.Cell(row, 2).Value = item.OrderItemId;
                worksheet.Cell(row, 3).Value = item.OrderRowId;
                worksheet.Cell(row, 4).Value = item.LineNumber;
                worksheet.Cell(row, 5).Value = item.ProductCode;
                worksheet.Cell(row, 6).Value = item.ProductDesc;
                worksheet.Cell(row, 7).Value = item.ListValue;
                worksheet.Cell(row, 8).Value = item.SaleValue;
                worksheet.Cell(row, 9).Value = item.Quantity;
                worksheet.Cell(row, 10).Value = item.ProductLevel1;
                worksheet.Cell(row, 11).Value = item.ProductLevel2;
                worksheet.Cell(row, 12).Value = item.ProductLevel3;
                worksheet.Cell(row, 13).Value = item.ProductType;
                worksheet.Cell(row, 14).Value = item.DateCreated;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, itemsFromDB.ToList().Count + 1, 16);
            dataRange.SetAutoFilter();

        }

        private void GetRevenuesSheet(XLWorkbook workbook, string year, string month = null)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = "Revenues payments ...", time = DateTime.Now });

            var worksheet = workbook.Worksheets.Add("Revenues");
            SetHeaders(worksheet, headersRevenues);

            var allocationTypeDict = db.ConfigAllocationTypes.Where(at => at.AllocationCriteriaSql == "2025").ToDictionary(at => at.AllocationTypeId, at => at.AllocationDescription);
            var revenuesFromDB = db.DataCreditAllocations.Where(d => d.PeriodYear == year).OrderBy(dc => dc.OrderId).ToList();
            if (!string.IsNullOrEmpty(month))
            {
                revenuesFromDB = revenuesFromDB.Where(d => d.PeriodYear == year && d.PeriodMonth == month).OrderBy(dc => dc.PeriodMonth).ToList();
            }

            var employeeIds = revenuesFromDB.Select(d => d.EmployeeId).Distinct().ToList();
            var orderIds = revenuesFromDB.Select(d => d.OrderId).Distinct().ToList();
            var adjustmentIds = revenuesFromDB.Where(d => d.AdjustmentId.HasValue).Select(d => d.AdjustmentId.Value).Distinct().ToList();

            var employeesDict = db.DataEmployees.Where(e => employeeIds.Contains(e.RowId)).ToDictionary(e => e.RowId, e => e.FstName + " " + e.LastName);
            var ordersDict = db.DataOrderHeaders.Where(o => orderIds.Contains(o.OrderRowId)).ToDictionary(o => o.OrderRowId, o => o);
            var adjustmentsDict = db.DataManualAdjustments.Where(a => adjustmentIds.Contains(a.AdjustmentId)).ToDictionary(a => a.AdjustmentId, a => a);
            var orderItemsDict = db.DataOrderItems.Where(i => orderIds.Contains(i.OrderRowId)).GroupBy(i => i.OrderRowId).ToDictionary(g => g.Key, g => g.ToList());

            int row = 2;

            foreach (var item in revenuesFromDB)
            {

                employeesDict.TryGetValue(item.EmployeeId, out var employeeName);
                ordersDict.TryGetValue(item.OrderId, out var order);
                orderItemsDict.TryGetValue(item.OrderId, out var itemsList);
                allocationTypeDict.TryGetValue(item.AllocationTypeId.Value, out var allocId);
                var productDesc = itemsList?.FirstOrDefault(p => p.OrderItemId == item.ItemId)?.ProductDesc;

                worksheet.Cell(row, 1).Value = item.DataCreditAllocationId;
                worksheet.Cell(row, 2).Value = $"{_dicoMonths[item.PeriodMonth]} {item.PeriodYear}";
                worksheet.Cell(row, 3).Value = item.AllocationValue ?? 0;
                worksheet.Cell(row, 4).Value = string.Empty;
                worksheet.Cell(row, 5).Value = employeeName ?? string.Empty;

                if (item.AdjustmentId.HasValue && adjustmentsDict.TryGetValue(item.AdjustmentId.Value, out var adjustment))
                {
                    worksheet.Cell(row, 6).Value = adjustment.AdjustmentReason ?? string.Empty;
                    worksheet.Cell(row, 10).Value = adjustment.OrderNumber ?? string.Empty;
                }
                else
                {
                    worksheet.Cell(row, 6).Value = string.Empty;
                    worksheet.Cell(row, 10).Value = string.Empty;
                }

                worksheet.Cell(row, 7).Value = order?.CustomerName ?? string.Empty;
                worksheet.Cell(row, 8).Value = order?.CustomerAccountNumber ?? string.Empty;
                worksheet.Cell(row, 9).Value = order?.OrderNumber ?? string.Empty;
                worksheet.Cell(row, 11).Value = productDesc ?? string.Empty;

                worksheet.Cell(row, 12).Value = item.AllocationSource ?? string.Empty;
                worksheet.Cell(row, 13).Value = string.Empty;
                worksheet.Cell(row, 14).Value = string.Empty;
                worksheet.Cell(row, 15).Value = allocId;
                worksheet.Cell(row, 16).Value = item.AllocationTypeId;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, revenuesFromDB.ToList().Count + 1, 16);
            dataRange.SetAutoFilter();

        }

        private void GetPaymentsSheet(XLWorkbook workbook, string year, string month = null)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = "Processing payments ...", time = DateTime.Now });

            var worksheet = workbook.Worksheets.Add("Payments");

            var paymentsFromDB = db.DataCommissionPayments.Where(dc => dc.PeriodYear == year).OrderBy(dc => dc.OrderId).ToList();
            if (!string.IsNullOrEmpty(month))
            {
                paymentsFromDB = paymentsFromDB.Where(dc => dc.PeriodMonth == month).OrderBy(dc => dc.PeriodMonth).ToList();
            }
            var employeeIds = paymentsFromDB.Select(dc => dc.EmployeeId).Distinct().ToList();
            var orderIds = paymentsFromDB.Where(dc => dc.OrderId != null).Select(dc => dc.OrderId).Distinct().ToList();
            var adjustmentIds = paymentsFromDB.Where(d => d.AdjustmentId.HasValue).Select(d => d.AdjustmentId.Value).Distinct().ToList();
            var employeesDict = db.DataEmployees.Where(e => employeeIds.Contains(e.RowId)).ToDictionary(e => e.RowId, e => e.FstName + " " + e.LastName);
            var ordersDict = db.DataOrderHeaders.Where(o => o.OrderRowId != null).Where(o => orderIds.Contains(o.OrderRowId)).ToDictionary(o => o.OrderRowId, o => o);
            var adjustmentsDict = db.DataManualAdjustments.Where(a => adjustmentIds.Contains(a.AdjustmentId)).ToDictionary(a => a.AdjustmentId, a => a);

            SetHeaders(worksheet, headersPayments);

            int row = 2;

            foreach (var payment in paymentsFromDB)
            {
                employeesDict.TryGetValue(payment.EmployeeId, out var employeeName);

                ordersDict.TryGetValue(payment.OrderId ?? string.Empty, out var order);


                string adjOrderNumber = string.Empty;
                string reason = string.Empty;

                if (payment.AdjustmentId.HasValue && adjustmentsDict.TryGetValue(payment.AdjustmentId.Value, out var adj))
                {
                    if (!string.IsNullOrWhiteSpace(adj.OrderNumber) &&
                        !adj.OrderNumber.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        adjOrderNumber = adj.OrderNumber.Trim();
                    }
                    else
                    {
                        adjOrderNumber = string.Empty;
                    }

                    reason = adj.AdjustmentReason ?? string.Empty;
                }

                worksheet.Cell(row, 1).Value = _dicoMonths.ContainsKey(payment.PeriodMonth) ? _dicoMonths[payment.PeriodMonth] + " " + payment.PeriodYear : "Unknown " + payment.PeriodYear;
                worksheet.Cell(row, 2).Value = employeeName ?? string.Empty;
                worksheet.Cell(row, 3).Value = payment.DataCommissionPaymentId;
                worksheet.Cell(row, 4).Value = order?.CustomerAccountNumber ?? string.Empty;
                worksheet.Cell(row, 5).Value = order?.CustomerName ?? string.Empty;
                worksheet.Cell(row, 6).Value = order?.OrderNumber ?? string.Empty;
                worksheet.Cell(row, 7).Value = adjOrderNumber != null ? adjOrderNumber : string.Empty;
                worksheet.Cell(row, 8).Value = !string.IsNullOrEmpty(reason) ? reason : payment.PaymentDescription;
                worksheet.Cell(row, 9).Value = payment.PaymentSource;
                worksheet.Cell(row, 10).Value = payment.PaymentRate;
                worksheet.Cell(row, 11).Value = payment.PaymentValue;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            var dataRange = worksheet.Range(1, 1, row - 1, 11);
            dataRange.SetAutoFilter();

        }

        private void SetHeaders(IXLWorksheet worksheet, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
        }

    }
}
