namespace SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

public class MyApprenticeship
{
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }

    public TrainingCourse? TrainingCourse { get; set; }

    public static implicit operator MyApprenticeship(MyApprenticeshipResponse myApprenticeshipResponse)
    {
        return new MyApprenticeship
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