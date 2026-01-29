using ICMServer.Helpers;

namespace ICMServer.Services
{
    public interface IPeriodContext
    {
        Task<string> GetPeriodYearAsync();
        Task<string> GetPeriodMonthAsync();
        string GetPeriodYear();
        string GetPeriodMonth();
    }

    public class PeriodContextService : IPeriodContext
    {
        private readonly ISystemParametersHelper _sysParams;
        private string? _periodYear;
        private string? _periodMonth;

        public PeriodContextService(ISystemParametersHelper sysParams)
        {
            _sysParams = sysParams;
        }

        public async Task<string> GetPeriodYearAsync()
        {
            return _periodYear ??= await _sysParams.GetSysParamAsync("PERIOD_YEAR");
        }

        public async Task<string> GetPeriodMonthAsync()
        {
            return _periodMonth ??= await _sysParams.GetSysParamAsync("PERIOD_MONTH");
        }


        public string GetPeriodYear()
        {
            return _periodYear ??= _sysParams.GetSysParam("PERIOD_YEAR");
        }

        public string GetPeriodMonth()
        {
            return _periodMonth ??= _sysParams.GetSysParam("PERIOD_MONTH");
        }
    }


}