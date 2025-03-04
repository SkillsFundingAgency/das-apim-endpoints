namespace SFA.DAS.ToolsSupport.Interfaces;

public interface IDatetimeService
{
    int GetYear(DateTime endDate);
    DateTime GetBeginningFinancialYear(DateTime endDate);
}
