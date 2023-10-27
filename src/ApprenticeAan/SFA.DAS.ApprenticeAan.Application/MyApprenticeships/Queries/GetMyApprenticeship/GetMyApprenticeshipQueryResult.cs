using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQueryResult
{
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }

    public TrainingCourse? TrainingCourse { get; set; }

    public static implicit operator GetMyApprenticeshipQueryResult(GetMyApprenticeshipResponse myApprenticeshipResponse)
    {
        return new GetMyApprenticeshipQueryResult
        {
            Uln = myApprenticeshipResponse.Uln,
            ApprenticeshipId = myApprenticeshipResponse.ApprenticeshipId,
            EmployerName = myApprenticeshipResponse.EmployerName,
            StartDate = myApprenticeshipResponse.StartDate,
            EndDate = myApprenticeshipResponse.EndDate,
            TrainingProviderId = myApprenticeshipResponse.TrainingProviderId,
            TrainingProviderName = myApprenticeshipResponse.TrainingProviderName
        };
    }
}