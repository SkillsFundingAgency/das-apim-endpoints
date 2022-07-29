using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation
{
    public class CreateProviderLocationCommandHandler : IRequestHandler<CreateProviderLocationCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;

        public CreateProviderLocationCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
        {
            _courseManagementApiClient = courseManagementApiClient;
        }

        public async Task<Unit> Handle(CreateProviderLocationCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new ProviderLocationCreateRequest(request);
            await _courseManagementApiClient.PostWithResponseCode<int>(apiRequest);
            return Unit.Value;
        }
    }
}
