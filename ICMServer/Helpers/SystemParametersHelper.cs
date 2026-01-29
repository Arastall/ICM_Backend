using ICMServer.DBContext;
using Microsoft.EntityFrameworkCore;


namespace ICMServer.Helpers
{
    public interface ISystemParametersHelper
    {
        Task<string> GetSysParamAsync(string parameterName);
        string GetSysParam(string parameterName);
    }

    public class SystemParametersHelper : ISystemParametersHelper
    {

        private readonly ICMDBContext _context;
        private readonly ILogger<SystemParametersHelper> _logger;

        public SystemParametersHelper(ICMDBContext context,
            ILogger<SystemParametersHelper> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> GetSysParamAsync(string parameterName)
        {
            try
            {
                var parameter = await _context.ConfigSystemParameters
                    .Where(p => p.ParameterName == parameterName)
                    .Select(p => p.ParameterValue)
                    .FirstOrDefaultAsync();

                if (parameter == null)
                {
                    _logger.LogWarning("Parameter {ParameterName} not found in CONFIG_SYSTEM_PARAMETERS", parameterName);
                    return "Parameter not on table!";
                }

                return parameter;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving system parameter {ParameterName}", parameterName);
                throw;
            }
        }

        public string GetSysParam(string parameterName)
        {
            try
            {
                var parameter = _context.ConfigSystemParameters
                    .Where(p => p.ParameterName == parameterName)
                    .Select(p => p.ParameterValue)
                    .FirstOrDefault();

                if (parameter == null)
                {
                    _logger.LogWarning("Parameter {ParameterName} not found in CONFIG_SYSTEM_PARAMETERS", parameterName);
                    return "Parameter not on table!";
                }

                return parameter;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving system parameter {ParameterName}", parameterName);
                throw;
            }
        }
    }
}
