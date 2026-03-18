using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation
{
    public class GetLocationResult
    {
        public GetLocationsListItem Location { get; set; }
    }
}