using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;
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

public class WhenHandlingSubmitApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Application_Is_Submitted_To_Recruit(
        long vacancyReference,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        SubmitApplicationCommandHandler handler)
    {
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        applicationApiResponse.VacancyReference = vacancyReference;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                    )))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c => 
                    c.PostUrl.Contains(request.CandidateId.ToString())
                    && ((PostSubmitApplicationRequestData)c.Data).VacancyReference == vacancyReference
                ), true)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, ""));
        
        
        var actual = await handler.Handle(request, CancellationToken.None);
        
        actual.Should().BeTrue();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Updated_If_Submitted_To_Recruit(
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        SubmitApplicationCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl.Contains(request.CandidateId.ToString()) && c.GetUrl.Contains(request.ApplicationId.ToString()))))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c => 
                    c.PostUrl.Contains(request.CandidateId.ToString())
                ), true)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, ""));

        var actual = await handler.Handle(request, CancellationToken.None);

        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
            )), Times.Once
        );
        actual.Should().BeTrue();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Not_Updated_If_Not_Successfully_Submitted(
        long vacancyReference,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        SubmitApplicationCommandHandler handler)
    {
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        applicationApiResponse.VacancyReference = vacancyReference;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c => c.PostUrl.Contains(request.CandidateId.ToString())), true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.InternalServerError, "An error Occurred"));

        var actual = await handler.Handle(request, CancellationToken.None);

        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Not_Updated_And_Not_Submitted_To_Recruit_If_Application_Is_Not_Found(
        SubmitApplicationCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        SubmitApplicationCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.IsAny<GetApplicationApiRequest>()))
            .ReturnsAsync((GetApplicationApiResponse)null!);
        
        var actual = await handler.Handle(request, CancellationToken.None);
        
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostSubmitApplicationRequest>(), true), Times.Never);
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        actual.Should().BeFalse();
    }
}