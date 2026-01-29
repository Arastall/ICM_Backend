using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ICMServer.Helpers;
using ICMServer.Interfaces;
using ICMServer.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.ComponentModel;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdjustmentsController : Controller
    {
        private IRepository _repository;
        private readonly ILogger<AdjustmentsController> _logger;
        private readonly AdjustmentImportCache _importCache;

        public AdjustmentsController(ILogger<AdjustmentsController> logger, IRepository repository, AdjustmentImportCache importCache)
        {
            _logger = logger;
            _repository = repository;
            _importCache = importCache;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Alive !");
        }



        [HttpPost("check-adjustments")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CheckAdjustments(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File missing or wrong format");

            var adjustments = new List<DataManualAdjustment>();

            using (var stream = file.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheets.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed();

                foreach (var row in rows.Skip(1)) // Skip header row
                {
                    // Ignorer les lignes cachées (filtrées)
                    if (worksheet.Row(row.RowNumber()).IsHidden)
                        continue;

                    // Ignorer si la cellule B (colonne 2) n'est pas vide
                    var cellB = row.Cell(2).GetString().Trim();
                    if (!string.IsNullOrEmpty(cellB))
                        continue;

                    var adjustment = new DataManualAdjustment
                    {
                        AdjustmentType = row.Cell(5).GetString().Trim(),
                        PositionId = row.Cell(6).GetString().Trim(),
                        EmployeeId = row.Cell(7).GetString().Trim(),
                        OrderId = row.Cell(8).GetString().Trim(),
                        AllocationTypeId = row.Cell(9).TryGetValue<int?>(out var allocTypeId) ? allocTypeId : null,
                        AllocationValue = row.Cell(10).TryGetValue<decimal?>(out var allocVal) ? allocVal : null,
                        AllocationList = row.Cell(11).TryGetValue<decimal?>(out var allocList) ? allocList : null,
                        ExcludeFromCalcs = row.Cell(12).GetString().Trim() == "NULL" ? null : row.Cell(12).GetString().Trim(),
                        AdjustmentReason = row.Cell(13).GetString().Trim(),
                        AdjustmentDate = row.Cell(14).TryGetValue<DateTime>(out var adjDate) ? adjDate : DateTime.Now,
                        AdjustmentBy = row.Cell(15).GetString().Trim(),
                        AdjustmentProcessed = row.Cell(16).GetString().Trim(),
                        PeriodMonth = row.Cell(17).GetString().Trim(),
                        PeriodYear = row.Cell(18).GetString().Trim(),
                        DateCreated = row.Cell(19).TryGetValue<DateTime>(out var created) ? created : DateTime.Now,
                        OrderNumber = row.Cell(20).GetString().Trim(),
                        ItIssue = row.Cell(21).TryGetValue<int>(out var issue) ? issue : 0,
                        AllocationRate = row.Cell(22).TryGetValue<decimal?>(out var rate) ? rate : null
                    };

                    adjustments.Add(adjustment);
                }
            }

            // Stocke en cache et retourne la clé
            var cacheKey = _importCache.Store(adjustments);

            return Ok(new
            {
                cacheKey,
                count = adjustments.Count,
                adjustments
            });
        }

        [HttpPost("validate-adjustments/{cacheKey}")]
        public async Task<IActionResult> ValidateAdjustments(string cacheKey)
        {
            var adjustments = _importCache.Get(cacheKey);

            if (adjustments == null)
                return BadRequest("Session expired or invalid. Please re-upload the file.");

            _repository.InsertAdjustmentsAsync(adjustments);

            _importCache.Remove(cacheKey); // Nettoie le cache

            return Ok(new { inserted = adjustments.Count });
        }

    }
}
