using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteCandidate;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands
{
    [TestFixture]
    public class WhenHandlingDeleteCandidateCommand
    {
        [Test]
        [MoqInlineAutoData("address1", "address2", "address3", "address4", "address4")]
        [MoqInlineAutoData("address1", "address2", "address3", null, "address3")]
        [MoqInlineAutoData("address1", "address2", null, null, "address2")]
        [MoqInlineAutoData("address1", null, null, null, "address1")]
        public async Task Then_The_Application_Is_Withdrawn_From_Recruit_Status_Updated_And_Email_Sent_AccountDeleted(
                string address1,
                string address2,
                string address3,
                string address4,
                string expectedAddress,
                long vacancyRef,
                DeleteCandidateCommand command,
                GetApplicationsApiResponse applicationsApiResponse,
                GetCandidateApiResponse candidateApiResponse,
                EmailEnvironmentHelper emailEnvironmentHelper,
                GetApprenticeshipVacancyItemResponse vacancyResponse,
                [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
                [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
                [Frozen] Mock<INotificationService> notificationService,
                [Frozen] Mock<IVacancyService> vacancyService,
                [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
                DeleteCandidateCommandHandler handler)
        {
            foreach (var application in applicationsApiResponse.Applications)
            {
                application.VacancyReference = $"VAC{vacancyRef}";
                application.Status = ApplicationStatus.Submitted.ToString();
                application.CandidateId = command.CandidateId;
            }
            vacancyResponse.Address.AddressLine1 = address1;
            vacancyResponse.Address.AddressLine2 = address2;
            vacancyResponse.Address.AddressLine3 = address3;
            vacancyResponse.Address.AddressLine4 = address4;

            var expectedGetApplicationsRequest =
                new GetApplicationsApiRequest(command.CandidateId);
            candidateApiClient
                .Setup(x => x.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(c =>
                        c.GetUrl == expectedGetApplicationsRequest.GetUrl
                    )))
                .ReturnsAsync(applicationsApiResponse);


            var expectedGetCandidateRequest =
                new GetCandidateApiRequest(command.CandidateId.ToString());
            candidateApiClient
                .Setup(x => x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c =>
                        c.GetUrl == expectedGetCandidateRequest.GetUrl
                    )))
                .ReturnsAsync(candidateApiResponse);

            candidateApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>())).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
            recruitApiClient
                .Setup(x => x.PostWithResponseCode<NullResponse>(
                    It.IsAny<PostWithdrawApplicationRequest>(), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));
            vacancyService.Setup(x => x.GetVacancy($"VAC{vacancyRef}")).ReturnsAsync(vacancyResponse);

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);

            foreach (var application in applicationsApiResponse.Applications)  
            {
                candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(application.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(command.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
                )), Times.AtLeastOnce);

                recruitApiClient
                    .Verify(x => x.PostWithResponseCode<NullResponse>(
                        It.Is<PostWithdrawApplicationRequest>(c =>
                            c.PostUrl.Contains(command.CandidateId.ToString())
                            && c.PostUrl.Contains(vacancyRef.ToString())
                        ), false), Times.Exactly(applicationsApiResponse.Applications.Count));

                notificationService.Verify(x => x.Send(
                    It.Is<SendEmailCommand>(c =>
                        c.RecipientsAddress == candidateApiResponse.Email
                        && c.TemplateId == emailEnvironmentHelper.WithdrawApplicationEmailTemplateId
                        && c.Tokens["firstName"] == candidateApiResponse.FirstName
                        && c.Tokens["vacancy"] == vacancyResponse.Title
                        && c.Tokens["employer"] == vacancyResponse.EmployerName
                        && c.Tokens["location"] == $"{expectedAddress}, {vacancyResponse.Address.Postcode}"
                    )
                ), Times.Exactly(applicationsApiResponse.Applications.Count));
            }

            var expectedDeleteSavedSearchesApiRequest = new DeleteSavedSearchesApiRequest(command.CandidateId);
            findApprenticeshipApiClient.Verify(client => client.Delete(It.Is<DeleteSavedSearchesApiRequest>(r => r.DeleteUrl == expectedDeleteSavedSearchesApiRequest.DeleteUrl)), Times.Once);

            var expectedDeleteAccountApiRequest =
                new DeleteAccountApiRequest(command.CandidateId);
            candidateApiClient
                .Verify(client => client.Delete(It.Is<DeleteAccountApiRequest>(r => r.DeleteUrl == expectedDeleteAccountApiRequest.DeleteUrl)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Submitted_Applications_Returns_Unit(
            long vacancyRef,
            DeleteCandidateCommand command,
            GetApplicationsApiResponse applicationsApiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<INotificationService> notificationService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            DeleteCandidateCommandHandler handler)
        {
            foreach (var application in applicationsApiResponse.Applications)
            {
                application.VacancyReference = $"VAC{vacancyRef}";
                application.Status = ApplicationStatus.Withdrawn.ToString();
                application.CandidateId = command.CandidateId;
            }

            var expectedGetApplicationsRequest =
                new GetApplicationsApiRequest(command.CandidateId);
            candidateApiClient
                .Setup(x => x.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(c =>
                        c.GetUrl == expectedGetApplicationsRequest.GetUrl
                    )))
                .ReturnsAsync(applicationsApiResponse);


            await handler.Handle(command, CancellationToken.None);

            candidateApiClient.Verify(x => x.Delete(It.IsAny<DeleteAccountApiRequest>()), Times.Once());
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
            notificationService.Verify(x => x.Send(
                It.IsAny<SendEmailCommand>()), Times.Never());

            findApprenticeshipApiClient.Verify(client => client.Delete(It.IsAny<DeleteSavedSearchesApiRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Applications_Returns_Unit(
            long vacancyRef,
            DeleteCandidateCommand command,
            GetApplicationsApiResponse applicationsApiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<INotificationService> notificationService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            DeleteCandidateCommandHandler handler)
        {
            applicationsApiResponse.Applications = [];

            var expectedGetApplicationsRequest =
                new GetApplicationsApiRequest(command.CandidateId);
            candidateApiClient
                .Setup(x => x.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(c =>
                        c.GetUrl == expectedGetApplicationsRequest.GetUrl
                    )))
                .ReturnsAsync(applicationsApiResponse);


            await handler.Handle(command, CancellationToken.None);

            candidateApiClient.Verify(x => x.Delete(It.IsAny<DeleteAccountApiRequest>()), Times.Once());
            findApprenticeshipApiClient.Verify(client => client.Delete(It.IsAny<DeleteSavedSearchesApiRequest>()), Times.Once);
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
            notificationService.Verify(x => x.Send(
                It.IsAny<SendEmailCommand>()), Times.Never());
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Successful_To_Recruit_Returns_Exception(
            long vacancyRef,
            DeleteCandidateCommand command,
            GetApplicationsApiResponse applicationsApiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<INotificationService> notificationService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            DeleteCandidateCommandHandler handler)
        {
            foreach (var application in applicationsApiResponse.Applications)
            {
                application.VacancyReference = $"VAC{vacancyRef}";
                application.Status = ApplicationStatus.Submitted.ToString();
                application.CandidateId = command.CandidateId;
            }

            var expectedGetApplicationsRequest =
                new GetApplicationsApiRequest(command.CandidateId);
            candidateApiClient
                .Setup(x => x.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(c =>
                        c.GetUrl == expectedGetApplicationsRequest.GetUrl
                    )))
                .ReturnsAsync(applicationsApiResponse);

            recruitApiClient
                .Setup(x => x.PostWithResponseCode<NullResponse>(
                    It.IsAny<PostWithdrawApplicationRequest>(), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.InternalServerError, ""));


            var act = async () => { await handler.Handle(command, CancellationToken.None); };
            await act.Should().ThrowAsync<HttpRequestContentException>();

            candidateApiClient.Verify(x => x.Delete(It.IsAny<DeleteAccountApiRequest>()), Times.Never());
            findApprenticeshipApiClient.Verify(client => client.Delete(It.IsAny<DeleteSavedSearchesApiRequest>()), Times.Never);
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
            notificationService.Verify(x => x.Send(
                It.IsAny<SendEmailCommand>()), Times.Never());
        }
    }
}
