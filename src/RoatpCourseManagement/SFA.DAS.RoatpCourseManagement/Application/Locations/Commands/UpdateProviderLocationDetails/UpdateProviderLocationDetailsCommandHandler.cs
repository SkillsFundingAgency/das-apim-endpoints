using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using System.Net;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.UpdateProviderLocationDetails
{
    public class UpdateProviderLocationDetailsCommandHandler : IRequestHandler<UpdateProviderLocationDetailsCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateProviderLocationDetailsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }
        public async Task<HttpStatusCode> Handle(UpdateProviderLocationDetailsCommand command, CancellationToken cancellationToken)
        {
            var updateProviderLocation = new ProviderLocationUpdateModel
            {
                Ukprn = command.Ukprn,
                Id = command.Id,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                LocationName = command.LocationName,
                Website = command.Website,
                Email = command.Email,
                Phone = command.Phone
            };

            var request = new ProviderLocationUpdateRequest(updateProviderLocation);
            await _innerApiClient.Put(request);
            return HttpStatusCode.NoContent;
        }
    }
}
