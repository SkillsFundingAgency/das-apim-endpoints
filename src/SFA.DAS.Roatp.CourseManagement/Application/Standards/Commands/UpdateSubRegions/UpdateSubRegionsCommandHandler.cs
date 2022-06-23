using MediatR;
using SFA.DAS.Roatp.CourseManagement.Application.Locations.Queries;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateSubRegions
{
    public class UpdateSubRegionsCommandHandler : IRequestHandler<UpdateSubRegionsCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateSubRegionsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }


        public async Task<HttpStatusCode> Handle(UpdateSubRegionsCommand command, CancellationToken cancellationToken)
        {
            var existingProviderLocation = await _innerApiClient.Get<List<ProviderLocationModel>>(new GetAllProviderLocationsQuery (command.Ukprn ));
            var existingSubregions = existingProviderLocation.FindAll(l => l.LocationType == LocationType.Regional);
            var newSubregionIdsToAdd = new List<int>();
            foreach (var regionId in command.SelectedSubRegions)
            {
                if (!existingSubregions.Exists(r => r.RegionId == int.Parse(regionId)))
                {
                    newSubregionIdsToAdd.Add(int.Parse(regionId));
                }
            }

            if(newSubregionIdsToAdd.Count == 0)
            {
                return HttpStatusCode.OK;
            }

            var updateProviderLocation = new ProviderLocationUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                SubregionIds = newSubregionIdsToAdd,
            };


            var request = new UpdateProviderLocationRequest(updateProviderLocation);
            var response = await _innerApiClient.PostWithResponseCode<UpdateProviderLocationRequest>(request);
            return response.StatusCode;
        }
    }
}
