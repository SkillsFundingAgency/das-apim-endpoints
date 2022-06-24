﻿using MediatR;
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

            var subregionIdsToDelete = new List<int>();
            foreach (var subregions in existingSubregions)
            {
                if (!command.SelectedSubRegions.ToList().Contains(subregions.RegionId.Value.ToString()))
                {
                    subregionIdsToDelete.Add(subregions.RegionId.Value);
                }
            }

            var updateProviderLocation = new ProviderLocationUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                SelectedSubregionIds = newSubregionIdsToAdd,
            };

            var requestProviderLocation = new UpdateProviderLocationRequest(updateProviderLocation);
            var responseProviderLocation = await _innerApiClient.PostWithResponseCode<UpdateProviderLocationRequest>(requestProviderLocation);

            var AllSelectedSubregionIdsToAdd = new List<int>();
            foreach (var regionId in command.SelectedSubRegions)
            {
                AllSelectedSubregionIdsToAdd.Add(int.Parse(regionId));
            }
            var updateProviderCourseLocation = new ProviderCourseLocationUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                SelectedSubregionIds = AllSelectedSubregionIdsToAdd,
            };

            var requestProviderCourseLocation = new UpdateProviderCourseLocationRequest(updateProviderCourseLocation);
            var responseProviderCourseLocation = await _innerApiClient.PostWithResponseCode<UpdateProviderCourseLocationRequest>(requestProviderCourseLocation);
           
            var deleteProviderLocation = new ProviderLocationDeleteModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                DeSelectedSubregionIds = subregionIdsToDelete,
            };

            var requestProviderLocationDelete = new DeleteProviderLocationRequest(deleteProviderLocation);
            var responseProviderLocationDelete = await _innerApiClient.PostWithResponseCode<DeleteProviderLocationRequest>(requestProviderLocationDelete);

            return HttpStatusCode.NoContent;
        }
    }
}
