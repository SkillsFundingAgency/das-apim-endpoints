using MediatR;
<<<<<<< HEAD:src/RoatpCourseManagement/SFA.DAS.Roatp.CourseManagement/Application/Standards/Commands/UpdateContactDetails/UpdateContactDetailsCommandHandler.cs
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
=======
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
>>>>>>> 06c8c044 (More separated pipelines, rename RoatpCourseManagement for alignment, clear up -old bits):src/RoatpCourseManagement/SFA.DAS.RoatpCourseManagement/Application/Standards/Commands/UpdateContactDetails/UpdateContactDetailsCommandHandler.cs
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails
{
    public class UpdateContactDetailsCommandHandler : IRequestHandler<UpdateContactDetailsCommand, HttpStatusCode>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateContactDetailsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<HttpStatusCode> Handle(UpdateContactDetailsCommand command, CancellationToken cancellationToken)
        {
            var providerCourse = await _innerApiClient.Get<GetProviderCourseResponse>(new GetProviderCourseRequest(command.Ukprn, command.LarsCode));
            var updateProviderCourse = new ProviderCourseUpdateModel
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                ContactUsEmail = command.ContactUsEmail,
                ContactUsPhoneNumber = command.ContactUsPhoneNumber,
                ContactUsPageUrl = command.ContactUsPageUrl,
                StandardInfoUrl = command.StandardInfoUrl,
                IsApprovedByRegulator = providerCourse.IsApprovedByRegulator
            };


            var request = new UpdateProviderCourseRequest(updateProviderCourse);
            var response = await _innerApiClient.PostWithResponseCode<UpdateProviderCourseRequest>(request);
            return response.StatusCode;
        }
    }
}
