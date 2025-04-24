using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations
{
    public record GetEmploymentLocationsQueryResult
    {
        public LocationDto EmploymentLocation { get; private init; }

        public static implicit operator GetEmploymentLocationsQueryResult(GetEmploymentLocationApiResponse source)
        {
            if (source is null) return null;
            return new GetEmploymentLocationsQueryResult
            {
                EmploymentLocation = source.EmploymentLocation,
            };
        }
    }
}