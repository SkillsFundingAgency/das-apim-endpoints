using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;

public class WhenHandlingWithdrawApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Application_Is_Withdrawn_From_Recruit_And_Status_Updated(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        WithdrawApplicationCommandHandler handler)
    {
        applicationApiResponse.VacancyReference = $"VAC{vacancyRef}";
        applicationApiResponse.Status = "Submitted";
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>())).ReturnsAsync(new ApiResponse<string>("",HttpStatusCode.Accepted,""));
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeTrue();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
        )), Times.Once);
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostWithdrawApplicationRequest>(c => 
                    c.PostUrl.Contains(request.CandidateId.ToString())
                    && c.PostUrl.Contains(vacancyRef.ToString())
                ), false), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Already_Withdrawn_Not_Submitted(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        WithdrawApplicationCommandHandler handler)
    {
        applicationApiResponse.VacancyReference = $"VAC{vacancyRef}";
        applicationApiResponse.Status = "Withdrawn";
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeFalse();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false), Times.Never);

    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Found_Then_Not_Submitted(
        WithdrawApplicationCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        WithdrawApplicationCommandHandler handler)
    {
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync((GetApplicationApiResponse)null);
        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeFalse();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Successful_To_Recruit_Returns_False(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        WithdrawApplicationCommandHandler handler)
    {
        applicationApiResponse.VacancyReference = $"VAC{vacancyRef}";
        applicationApiResponse.Status = "Submitted";
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.InternalServerError, ""));

        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeFalse();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
    }
}