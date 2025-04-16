using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations
{
    public record GetEmploymentLocationsQueryResult
    {
        public LocationDto EmploymentLocation { get; init; }
        public bool? IsSectionCompleted { get; init; }
    }
}