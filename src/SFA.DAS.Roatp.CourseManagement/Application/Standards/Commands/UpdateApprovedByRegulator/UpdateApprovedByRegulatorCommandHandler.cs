using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator
{
    public class UpdateApprovedByRegulatorCommandHandler : IRequestHandler<UpdateApprovedByRegulatorCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;

        public UpdateApprovedByRegulatorCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<HttpStatusCode> Handle(UpdateApprovedByRegulatorCommand command, CancellationToken cancellationToken)
        {
            var request = new UpdateApprovedByRegulatorRequest(command);
            var response = await _innerApiClient.PostWithResponseCode<UpdateApprovedByRegulatorRequest>(request);
            return response.StatusCode;
        }
    }
}
