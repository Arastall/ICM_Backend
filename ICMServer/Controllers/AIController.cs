using ICMServer.Classes;
using ICMServer.Interfaces;
using ICMServer.Managers;
using ICMServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIController : ControllerBase
    {
        private readonly AiAnalysisService _ai;

        public AIController(AiAnalysisService ai)
        {
            _ai = ai;
        }

        public class QuestionRequest
        {
            public string Question { get; set; }
        }

        [HttpPost("Analyze")]
        public async Task<IActionResult> Analyze([FromBody] QuestionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Question is required.");

            var result = await _ai.AnalyzeDataAsync(request.Question);
            var content = Content(result, "text/html");
            return Ok(content);
        }

    }

}
