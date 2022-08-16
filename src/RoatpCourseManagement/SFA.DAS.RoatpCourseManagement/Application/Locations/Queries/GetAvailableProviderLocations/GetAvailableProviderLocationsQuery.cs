using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations
{
    public class GetAvailableProviderLocationsQuery : IRequest<GetAvailableProviderLocationsQueryResult>
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public GetAvailableProviderLocationsQuery(int ukprn, int larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}
