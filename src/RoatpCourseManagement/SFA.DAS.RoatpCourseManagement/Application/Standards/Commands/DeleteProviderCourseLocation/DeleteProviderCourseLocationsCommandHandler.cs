using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation
{
    public class DeleteProviderCourseLocationsCommandHandler : IRequestHandler<DeleteProviderCourseLocationCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;

        public DeleteProviderCourseLocationsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<Unit> Handle(DeleteProviderCourseLocationCommand request, CancellationToken cancellationToken)
        {
            await _innerApiClient.Delete((DeleteProviderCourseLocationRequest)request);
            return Unit.Value;
        }
    }
}
