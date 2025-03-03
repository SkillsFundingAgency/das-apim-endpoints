using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Candidates.Queries.GetCandidate;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Candidates.Queries;

public class WhenHandlingGetCandidateQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Api_Called_To_Get_Candidate(
        GetCandidateQuery query,
        GetCandidateApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidateQueryHandler handler)
    {
        var expectedRequest = new GetCandidateByIdApiRequest(query.CandidateId);
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateByIdApiRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidate.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_If_No_Candidate_Found_Null_Returned(
        GetCandidateQuery query,
        GetCandidateApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidateQueryHandler handler)
    {
        var expectedRequest = new GetCandidateByIdApiRequest(query.CandidateId);
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateByIdApiRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidate.Should().BeNull();
    }
}