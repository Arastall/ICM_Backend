using DocumentFormat.OpenXml.Bibliography;
using ICMServer.Classes;
using ICMServer.Models;
using System.Collections.Generic;

namespace ICMServer.Interfaces
{
    public interface IRepository
    {
        List<Menu> GetMenuList();

        Task RunICMFrontend();

        Task RunICM();

        Task RunICMOld();

        object GetLastRunInfo();

        MainInfo GetMainInfo();

        DataEmployee GetEmployee(string id);

        DataEmployee SearchEmployeeBySurname(string surname);

        List<DataEmployee> SearchEmployee(string value);

        List<AchievementHistory> GetAchievementHistory(string employeeId);

        void InsertAdjustmentsAsync(List<DataManualAdjustment> adjustments);

        /* Report Controller */
        byte[] GetYearlyRevenuesPaymentsReport(string year);

        byte[] GetMonthRevenuesPaymentsReport(string year, string month);

        byte[] GetMonthlyDealsInfoReport(string year, string month);

        //byte[] GetCurrentSPPayFile();
        byte[] GetPayFileReport(string year, string month);

        byte[] GetPayFileReportOld(string year, string month);

        byte[] GetOrderSequenceFromLogs(int runID);

        /* Settings Controller */
        object GetCurrentSalesPeriod();
        string GetCurrentSalesPeriodStr();

        bool SetCurrentSalesPeriod(string year, string month);


    }
}
