using ICMServer.Classes;
using ICMServer.Interfaces;
using ICMServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private IRepository _repository;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(ILogger<EmployeesController> logger, IRepository repository, IEmployeeService employeeService)
        {
            _logger = logger;
            _repository = repository;
            _employeeService = employeeService;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Alive !");
        }

        [HttpGet]
        [Route("GetEmployee/{employeeId}")]
        public ActionResult<DataEmployee> GetEmployee(string employeeId)
        {
            _logger.LogInformation($"Get employee {employeeId}");
            var employee = _repository.GetEmployee(employeeId);
            return Ok(employee);
        }


        [HttpGet]
        [Route("SearchEmployeeBySurname/{surname}")]
        public ActionResult<DataEmployee> SearchEmployeeBySurname(string surname)
        {
            _logger.LogInformation($"Search employee with {surname}");
            var employee = _repository.SearchEmployeeBySurname(surname);
            return Ok(employee);
        }

        [HttpGet]
        [Route("SearchEmployee/{value}")]
        public ActionResult<List<DataEmployee>> SearchEmployee(string value)
        {
            _logger.LogInformation($"Search employee with {value}");
            var employees = _repository.SearchEmployee(value);
            return Ok(employees);
        }

        [HttpGet]
        [Route("GetAchievementHistory/{employeeId}")]
        public ActionResult<List<AchievementHistory>> GetAchievementHistory(string employeeId)
        {
            _logger.LogInformation($"Get achivement history for employee {employeeId}");
            var history = _repository.GetAchievementHistory(employeeId);
            return Ok(history);
        }

        //[HttpPost]
        //[Route("SavePatient")]
        //public ActionResult<Patient> SavePatient([FromBody] Patient patient)
        //{
        //    _logger.LogInformation($"Save patient {patient.Prenom} {patient.Nom}");
        //    var savedPatient = _repository.SavePatient(patient);
        //    return Ok(savedPatient);
        //}

        //[HttpDelete]
        //[Route("DeletePatient/{patientId}")]
        //public ActionResult<bool> DeletePatient(int patientId)
        //{
        //    _logger.LogInformation($"Deleting patient {patientId}");
        //    var result = _repository.DeletePatient(patientId);
        //    return Ok(result);
        //}

        [HttpGet("GetEmployeeSummary/{login}/{year}/{month}")]
        public async Task<IActionResult> GetEmployeeSummary(string login, string year, string month)
        {
            if (string.IsNullOrWhiteSpace(login))
                return BadRequest("Login is required.");

            var summary = await _employeeService.GetEmployeeSummaryAsync(login.Trim(), year.Trim(), month.Trim());
            if (summary == null)
                return NotFound(new { message = $"Employee with login '{login}' not found." });

            return Ok(summary);
        }
    }
}
