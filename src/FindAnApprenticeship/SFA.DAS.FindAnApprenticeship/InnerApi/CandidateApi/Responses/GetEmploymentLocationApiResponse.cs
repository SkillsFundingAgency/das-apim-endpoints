using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public record GetEmploymentLocationApiResponse
    {
        public LocationDto EmploymentLocation { get; init; }
    }
}