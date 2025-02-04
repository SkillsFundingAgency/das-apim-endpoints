namespace SFA.DAS.EmployerAan.Application.Onboarding.ConfirmDetails.Queries;

public class GetOnboardingConfirmDetailsQueryResult
{
    public int NumberOfActiveApprentices { get; set; }
    public List<string> Sectors { get; set; } = [];
}