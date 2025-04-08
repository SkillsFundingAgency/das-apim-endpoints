using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations
{
    public record GetEmploymentLocationsQueryResult
    {
        public List<LocationDto> EmploymentLocations { get; private init; } = [];

        public static implicit operator GetEmploymentLocationsQueryResult(GetEmploymentLocationsApiResponse source)
        {
            if (source is null) return null;
            return new GetEmploymentLocationsQueryResult
            {
                EmploymentLocations = source.EmploymentLocations,
            };
        }
    }
}