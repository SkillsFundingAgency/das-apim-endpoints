namespace SFA.DAS.AdminAan.Domain;

public class GetEmployerApprenticeshipsSummaryResponse
{
    public IEnumerable<DateTime> StartDates { get; set; } = Enumerable.Empty<DateTime>();
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}
