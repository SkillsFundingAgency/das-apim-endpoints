using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.AddNationalLocation
{
    public class AddNationalLocationToProviderCourseLocationsCommandHandler : IRequestHandler<AddNationalLocationToProviderCourseLocationsCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public AddNationalLocationToProviderCourseLocationsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<Unit> Handle(AddNationalLocationToProviderCourseLocationsCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new AddNationalLocationToProviderCourseLocationsRequest(request.Ukprn, request.LarsCode, request.UserId, request.UserDisplayName);
            await _innerApiClient.PostWithResponseCode<int>(apiRequest);
            return Unit.Value;
        }
    }
}
