using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse
{
    public class DeleteProviderCourseCommandHandler : IRequestHandler<DeleteProviderCourseCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;

        public DeleteProviderCourseCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<Unit> Handle(DeleteProviderCourseCommand request, CancellationToken cancellationToken)
        {
            await _innerApiClient.Delete((DeleteProviderCourseRequest)request);
            return Unit.Value;
        }
    }
}
