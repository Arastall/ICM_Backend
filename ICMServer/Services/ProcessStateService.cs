using System.Text.Json.Serialization;

namespace ICMServer.Services
{
    public class ProcessStep
    {
        [JsonPropertyName("stepId")]
        public string StepId { get; set; } = string.Empty;

        [JsonPropertyName("stepName")]
        public string StepName { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "pending";

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("time")]
        public DateTime? Time { get; set; }
    }

    public class ProcessState
    {
        [JsonPropertyName("isRunning")]
        public bool IsRunning { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime? EndTime { get; set; }

        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        [JsonPropertyName("finalMessage")]
        public string? FinalMessage { get; set; }

        [JsonPropertyName("steps")]
        public List<ProcessStep> Steps { get; set; } = new();
    }

    public interface IProcessStateService
    {
        void StartProcess(List<ProcessStep> steps);
        void UpdateStep(string stepId, string stepName, string status, string message);
        void FinishProcess(bool success, string message);
        void Reset();
        ProcessState GetState();
    }

    public class ProcessStateService : IProcessStateService
    {
        private readonly object _lock = new();
        private ProcessState _state = new();

        public void StartProcess(List<ProcessStep> steps)
        {
            lock (_lock)
            {
                _state = new ProcessState
                {
                    IsRunning = true,
                    StartTime = DateTime.Now,
                    Steps = steps
                };
            }
        }

        public void UpdateStep(string stepId, string stepName, string status, string message)
        {
            lock (_lock)
            {
                var step = _state.Steps.FirstOrDefault(s => s.StepId == stepId);
                if (step != null)
                {
                    step.StepName = stepName;
                    step.Status = status;
                    step.Message = message;
                    step.Time = DateTime.Now;
                }
                else
                {
                    _state.Steps.Add(new ProcessStep
                    {
                        StepId = stepId,
                        StepName = stepName,
                        Status = status,
                        Message = message,
                        Time = DateTime.Now
                    });
                }
            }
        }

        public void FinishProcess(bool success, string message)
        {
            lock (_lock)
            {
                _state.IsRunning = false;
                _state.EndTime = DateTime.Now;
                _state.Success = success;
                _state.FinalMessage = message;
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                _state = new ProcessState();
            }
        }

        public ProcessState GetState()
        {
            lock (_lock)
            {
                return new ProcessState
                {
                    IsRunning = _state.IsRunning,
                    StartTime = _state.StartTime,
                    EndTime = _state.EndTime,
                    Success = _state.Success,
                    FinalMessage = _state.FinalMessage,
                    Steps = _state.Steps.Select(s => new ProcessStep
                    {
                        StepId = s.StepId,
                        StepName = s.StepName,
                        Status = s.Status,
                        Message = s.Message,
                        Time = s.Time
                    }).ToList()
                };
            }
        }
    }
}
