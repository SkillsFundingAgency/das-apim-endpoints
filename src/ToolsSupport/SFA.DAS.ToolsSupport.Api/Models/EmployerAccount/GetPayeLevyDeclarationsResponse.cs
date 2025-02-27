using System.Globalization;
using SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.sources.EmployerAccount;

public class GetPayeLevyDeclarationsResponse
{
    public string PayeSchemeName { get; set; } = "";
    public string PayeSchemeFormatedAddedDate { get; set; } = "";
    public string PayeSchemeRef { get; set; } = "";
    public List<DeclarationResponse> LevyDeclarations { get; set; } = [];
    public bool UnexpectedError { get; set; }


    public static explicit operator GetPayeLevyDeclarationsResponse(GetPayeSchemeLevyDeclarationsResult source)
    {
        var payeLevyDeclaration = new GetPayeLevyDeclarationsResponse();

        if (source == null)
        {
            return payeLevyDeclaration;
        }

        payeLevyDeclaration.UnexpectedError = source.StatusCode == PayeLevySubmissionsResponseCodes.UnexpectedError;
        payeLevyDeclaration.PayeSchemeName = source.PayeScheme?.Name;
        payeLevyDeclaration.PayeSchemeRef = source.PayeScheme?.ObscuredPayeRef;
        payeLevyDeclaration.LevyDeclarations = source.LevySubmissions?.Declarations?.Select(o => MapLevyDeclarationViewModel(o)).ToList();
        payeLevyDeclaration.PayeSchemeFormatedAddedDate = source.PayeScheme?.AddedDate == DateTime.MinValue ?
                                      string.Empty :
                                      ConvertDateTimeToDdmmyyyyFormat(source.PayeScheme.AddedDate);
        return payeLevyDeclaration;
    }

    private static DeclarationResponse MapLevyDeclarationViewModel(Declaration declaration)
    {

        var levy = new DeclarationResponse
        {
            SubmissionDate = GetSubmissionDate(declaration.SubmissionTime),
            PayrollDate = GetPayrollDate(declaration.PayrollPeriod),
            LevySubmissionId = declaration.Id,
            YearToDateAmount = GetYearToDateAmount(declaration)
        };

        return levy;
    }

    private static string GetYearToDateAmount(Declaration levyDeclaration)
    { 
        return $"£{levyDeclaration.LevyDueYTD:#,##0.00}";
    }

    private static string ConvertDateTimeToDdmmyyyyFormat(DateTime? dateTime)
    {
        return dateTime?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    private static string GetSubmissionDate(DateTime submissionTime)
    {
        return submissionTime.ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
    }

    private static string GetPayrollDate(PayrollPeriod payrollPeriod)
    {
        if (payrollPeriod == null)
        {
            return string.Empty;
        }

        var month = payrollPeriod.Month + 3;

        if (month > 12)
        {
            month -= 12;
        }

        var monthName = new DateTime(2010, month, 1).ToString("MMM", CultureInfo.InvariantCulture);

        return $"{payrollPeriod.Year} {payrollPeriod.Month} ({monthName})";
    }
}

public class DeclarationResponse
{
    public string SubmissionDate { get; set; }
    public string PayrollDate { get; set; }
    public long LevySubmissionId { get; set; }
    public string YearToDateAmount { get; set; }
}