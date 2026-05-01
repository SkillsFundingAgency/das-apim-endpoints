using MediatR;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations
{
    public class GetAllProviderLocationsQuery : IGetApiRequest, IRequest<GetAllProviderLocationsQueryResult>
    {
        public string GetUrl => $"providers/{Ukprn}/locations";
        public int Ukprn { get; }

        public GetAllProviderLocationsQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
