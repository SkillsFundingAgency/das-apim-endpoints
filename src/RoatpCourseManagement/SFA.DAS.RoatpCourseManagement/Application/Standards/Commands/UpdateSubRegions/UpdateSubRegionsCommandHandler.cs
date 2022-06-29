using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.DeleteProviderCourseLocations;

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
            var existingProviderLocation = await _innerApiClient.Get<List<ProviderLocationModel>>(new GetAllProviderLocationsQuery(command.Ukprn));
            var existingSubregions = existingProviderLocation.FindAll(l => l.LocationType == LocationType.Regional);
            List<int> newSubregionIdsToAdd = GetProviderLocationsToAdd(command, existingSubregions);
            List<int> subregionIdsToDelete = GetProviderLocationsToDelete(command, existingSubregions);

            var providerLocationBulkInsertModel = new ProviderLocationBulkInsertModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                SelectedSubregionIds = newSubregionIdsToAdd,
            };

            var providerLocationsBulkInsertRequest = new ProviderLocationsBulkInsertRequest(providerLocationBulkInsertModel);
            await _innerApiClient.PostWithResponseCode<ProviderLocationsBulkInsertRequest>(providerLocationsBulkInsertRequest);

            var providerCourseLocationsBulkDeleteRequest = new ProviderCourseLocationsBulkDeleteRequest
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                DeleteProviderCourseLocationOption = DeleteProviderCourseLocationOption.DeleteEmployerLocations
            };
            await _innerApiClient.Delete(providerCourseLocationsBulkDeleteRequest);

            var providerCourseLocationBulkInsertModel = new ProviderCourseLocationBulkInsertModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                SelectedSubregionIds = command.SelectedSubRegions,
            };

            var providerCourseLocationBulkInsertRequest = new ProviderCourseLocationBulkInsertRequest(providerCourseLocationBulkInsertModel);
            await _innerApiClient.PostWithResponseCode<ProviderCourseLocationBulkInsertRequest>(providerCourseLocationBulkInsertRequest);

            var providerLocationBulkDeleteModel = new ProviderLocationBulkDeleteModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
            };

            var providerLocationBulkDeleteRequest = new ProviderLocationBulkDeleteRequest(providerLocationBulkDeleteModel);
            await _innerApiClient.PostWithResponseCode<ProviderLocationBulkDeleteRequest>(providerLocationBulkDeleteRequest);

            return HttpStatusCode.NoContent;
        }

        private static List<int> GetProviderLocationsToDelete(UpdateSubRegionsCommand command, List<ProviderLocationModel> existingSubregions)
        {
            var subregionIdsToDelete = new List<int>();
            foreach (var subregions in existingSubregions)
            {
                if (!command.SelectedSubRegions.ToList().Contains(subregions.RegionId.Value))
                {
                    subregionIdsToDelete.Add(subregions.RegionId.Value);
                }
            }

            return subregionIdsToDelete;
        }

        private static List<int> GetProviderLocationsToAdd(UpdateSubRegionsCommand command, List<ProviderLocationModel> existingSubregions)
        {
            var newSubregionIdsToAdd = new List<int>();
            foreach (var regionId in command.SelectedSubRegions)
            {
                if (!existingSubregions.Exists(r => r.RegionId == regionId))
                {
                    newSubregionIdsToAdd.Add(regionId);
                }
            }

            return newSubregionIdsToAdd;
        }
    }
}
