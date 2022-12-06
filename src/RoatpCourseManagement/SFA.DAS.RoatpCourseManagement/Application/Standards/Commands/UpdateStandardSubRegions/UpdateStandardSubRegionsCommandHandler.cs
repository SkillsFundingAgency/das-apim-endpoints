using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.DeleteProviderCourseLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions
{
    public class UpdateStandardSubRegionsCommandHandler : IRequestHandler<UpdateStandardSubRegionsCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateStandardSubRegionsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }
        public async Task<HttpStatusCode> Handle(UpdateStandardSubRegionsCommand command, CancellationToken cancellationToken)
        {
            var existingProviderLocation = await _innerApiClient.Get<List<ProviderLocationModel>>(new GetAllProviderLocationsQuery(command.Ukprn));
            var existingSubregions = existingProviderLocation.FindAll(l => l.LocationType == LocationType.Regional);
            List<int> newSubregionIdsToAdd = GetProviderLocationsToAdd(command, existingSubregions);
            if(newSubregionIdsToAdd.Count > 0)
            {
                await CreateProviderLocations(command, newSubregionIdsToAdd);
            }
            await DeleteExistingProviderCourseLocationRegions(command);
            await CreateProviderCourseLocationRegions(command);
            await CleanUpUnusedProviderLocations(command);

            return HttpStatusCode.NoContent;
        }

        private async Task CleanUpUnusedProviderLocations(UpdateStandardSubRegionsCommand command)
        {
            var providerLocationBulkDeleteModel = new ProviderLocationBulkDeleteModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
            };

            var providerLocationBulkDeleteRequest = new ProviderLocationBulkDeleteRequest(providerLocationBulkDeleteModel);
            await _innerApiClient.Delete(providerLocationBulkDeleteRequest);
        }

        private async Task CreateProviderCourseLocationRegions(UpdateStandardSubRegionsCommand command)
        {
            var providerCourseLocationBulkInsertModel = new ProviderCourseLocationBulkInsertModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                SelectedSubregionIds = command.SelectedSubRegions,
            };

            var providerCourseLocationBulkInsertRequest = new ProviderCourseLocationBulkInsertRequest(providerCourseLocationBulkInsertModel);
            await _innerApiClient.PostWithResponseCode<ProviderCourseLocationBulkInsertRequest>(providerCourseLocationBulkInsertRequest, false);
        }

        private async Task DeleteExistingProviderCourseLocationRegions(UpdateStandardSubRegionsCommand command)
        {
            var providerCourseLocationsBulkDeleteRequest = new ProviderCourseLocationsBulkDeleteRequest
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                DeleteProviderCourseLocationOption = DeleteProviderCourseLocationOption.DeleteEmployerLocations
            };
            await _innerApiClient.Delete(providerCourseLocationsBulkDeleteRequest);
        }

        private async Task CreateProviderLocations(UpdateStandardSubRegionsCommand command, List<int> newSubregionIdsToAdd)
        {
            var providerLocationBulkInsertModel = new ProviderLocationBulkInsertModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                SelectedSubregionIds = newSubregionIdsToAdd,
            };

            var providerLocationsBulkInsertRequest = new ProviderLocationsBulkInsertRequest(providerLocationBulkInsertModel);
            await _innerApiClient.PostWithResponseCode<ProviderLocationsBulkInsertRequest>(providerLocationsBulkInsertRequest, false);
        }

        private static List<int> GetProviderLocationsToAdd(UpdateStandardSubRegionsCommand command, List<ProviderLocationModel> existingSubregions)
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
