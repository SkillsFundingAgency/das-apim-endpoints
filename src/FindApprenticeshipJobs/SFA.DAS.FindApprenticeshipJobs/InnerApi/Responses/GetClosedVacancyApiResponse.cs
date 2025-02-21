namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

public class GetClosedVacancyApiResponse
{
    public string? Title { get; set; }
    public string? EmployerName { get; set; }
    public Address? EmployerLocation { get; set; }
}