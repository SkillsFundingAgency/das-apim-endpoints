using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public record GetEmploymentLocationsApiResponse
    {
        public List<LocationDto> EmploymentLocations { get; init; } = [];
    }
}