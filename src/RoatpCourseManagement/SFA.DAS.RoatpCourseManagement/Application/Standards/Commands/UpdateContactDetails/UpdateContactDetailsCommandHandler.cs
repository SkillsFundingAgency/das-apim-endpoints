using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails
{
    public class UpdateContactDetailsCommandHandler : IRequestHandler<UpdateContactDetailsCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateContactDetailsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }
    
        public async Task<Unit> Handle(UpdateContactDetailsCommand command, CancellationToken cancellationToken)
        {
            var patchUpdateProviderCourse = new ProviderCourseUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                ContactUsEmail = command.ContactUsEmail,
                ContactUsPhoneNumber = command.ContactUsPhoneNumber,
                ContactUsPageUrl = command.ContactUsPageUrl,
                StandardInfoUrl = command.StandardInfoUrl
            };
    
            var patchRequest = new PatchProviderCourseRequest(patchUpdateProviderCourse);
            await _innerApiClient.PatchWithResponseCode(patchRequest);
            return Unit.Value;
        }
    }
}
