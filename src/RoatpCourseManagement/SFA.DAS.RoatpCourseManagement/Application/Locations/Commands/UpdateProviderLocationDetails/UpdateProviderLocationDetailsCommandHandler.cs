using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;

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
            var existingProviderLocation = await _innerApiClient.Get<ProviderLocationModel>(new GetProviderLocationDetailsQuery(command.Ukprn, command.Id));
            return HttpStatusCode.NoContent;
        }
    }
}
