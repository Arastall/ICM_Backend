using ICMServer.DBContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Chat;

namespace ICMServer.Managers
{
    public class AiAnalysisService
    {
        private readonly ICMDBContext _context;
        private readonly OpenAIClient _openai;
        private const string OpenAiAPIKey = ""; // Set via environment variable or config

        public AiAnalysisService(ICMDBContext context)
        {
            _context = context;
            _openai = new OpenAIClient(OpenAiAPIKey);
        }

        public async Task<string> AnalyzeDataAsync(string question)
        {
            var employees = await _context.DataEmployees.ToListAsync();
            var positions = await _context.DataPositionHistories.Where(p => !p.EndDt.HasValue).Distinct().ToListAsync();

            var chat = _openai.GetChatClient("gpt-4.1-mini");

            var messages = new ChatMessage[]
            {
                ChatMessage.CreateSystemMessage("You are a helpful financial data analyst."),
                ChatMessage.CreateUserMessage($"Here is the employees data: {JsonConvert.SerializeObject(employees)}"),
                ChatMessage.CreateUserMessage($"Here is the position of employees data: {JsonConvert.SerializeObject(positions)}"),
                ChatMessage.CreateUserMessage($"Answer this question with only a list of data in HTML ul li format. without anything else, neither comment from you : {question}")
            };

            var response = await chat.CompleteChatAsync(messages);

            return response.Value.Content[0].Text;
        }

    }

}
