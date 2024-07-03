using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Candidates.Commands;

public class WhenHandlingCandidateApplicationStatusCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called(
        CandidateApplicationStatusCommand request,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.Outcome = "successful";
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        
        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                ((ApplicationStatus)c.Data.Operations[1].value) == ApplicationStatus.Successful
            )), Times.Once
        );
    }
    [Test, MoqAutoData]
    public async Task Then_If_The_Application_Id_Is_Empty_The_Candidate_Is_Looked_Up_And_Application_Is_Looked_Up_By_CandidateId_And_VacancyReference(
        GetCandidateApiResponse apiCandidateResponse,
        GetApplicationByReferenceApiResponse apiResponse,
        CandidateApplicationStatusCommand request,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.ApplicationId = Guid.Empty;
        request.Outcome = "successful";
        var expectedGetRequestMigratedCandidate = new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId);
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateByMigratedCandidateIdApiRequest>(c => c.GetUrl == expectedGetRequestMigratedCandidate.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(apiCandidateResponse, HttpStatusCode.OK, ""));
        var expectedGetRequest = new GetApplicationByReferenceApiRequest(apiCandidateResponse.Id, request.VacancyReference);
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetApplicationByReferenceApiResponse>(
                    It.Is<GetApplicationByReferenceApiRequest>(c => c.GetUrl == expectedGetRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetApplicationByReferenceApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        
        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(apiResponse.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                ((ApplicationStatus)c.Data.Operations[1].value) == ApplicationStatus.Successful
            )), Times.Once
        );
    }
    [Test, MoqAutoData]
    public async Task Then_If_The_Application_Id_Is_Empty_The_Candidate_Is_Looked_Up_And_If_Not_Found_Then_Nothing_Updated(
        GetCandidateApiResponse apiCandidateResponse,
        GetApplicationByReferenceApiResponse apiResponse,
        CandidateApplicationStatusCommand request,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.ApplicationId = Guid.Empty;
        var expectedGetRequestMigratedCandidate = new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId);
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateByMigratedCandidateIdApiRequest>(c => c.GetUrl == expectedGetRequestMigratedCandidate.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, ""));
        
        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>(
            )), Times.Never
        );
    }
}