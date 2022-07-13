using MediatR;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator
{
    public class UpdateApprovedByRegulatorCommandHandler : IRequestHandler<UpdateApprovedByRegulatorCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateApprovedByRegulatorCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<Unit> Handle(UpdateApprovedByRegulatorCommand command, CancellationToken cancellationToken)
        {
            var providerCourse = await _innerApiClient.Get<GetProviderCourseResponse>(new GetProviderCourseRequest(command.Ukprn, command.LarsCode));
            var updateProviderCourse = new ProviderCourseUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                ContactUsEmail = providerCourse.ContactUsEmail,
                ContactUsPhoneNumber = providerCourse.ContactUsPhoneNumber,
                ContactUsPageUrl = providerCourse.ContactUsPageUrl,
                StandardInfoUrl = providerCourse.StandardInfoUrl,
                IsApprovedByRegulator = command.IsApprovedByRegulator
            };
            var request = new ProviderCourseUpdateRequest(updateProviderCourse);
            await _innerApiClient.Put(request);
            return Unit.Value;
        }
    }
}
