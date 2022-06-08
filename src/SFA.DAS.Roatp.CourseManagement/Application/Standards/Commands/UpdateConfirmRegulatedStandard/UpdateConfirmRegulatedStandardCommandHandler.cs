using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateConfirmRegulatedStandard
{
    public class UpdateConfirmRegulatedStandardCommandHandler : IRequestHandler<UpdateConfirmRegulatedStandardCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;

        public UpdateConfirmRegulatedStandardCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<HttpStatusCode> Handle(UpdateConfirmRegulatedStandardCommand command, CancellationToken cancellationToken)
        {
            var request = new UpdateConfirmRegulatedStandardRequest(command);
            var response = await _innerApiClient.PostWithResponseCode<UpdateConfirmRegulatedStandardRequest>(request);
            return response.StatusCode;
        }
    }
}
