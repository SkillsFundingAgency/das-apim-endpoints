using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetWorkExperiences;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;

[TestFixture]
public class WhenHandlingGetVolunteeringAndWorkExperiencesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetVolunteeringAndWorkExperiencesQuery query,
        GetWorkHistoriesApiResponse workHistoriesApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetVolunteeringAndWorkExperiencesQueryHandler handler)
    {
        var expectedGetWorkHistoriesRequest = new GetWorkHistoriesApiRequest(query.ApplicationId, query.CandidateId, WorkHistoryType.WorkExperience);
        candidateApiClient
            .Setup(client => client.Get<GetWorkHistoriesApiResponse>(
                It.Is<GetWorkHistoriesApiRequest>(r => r.GetUrl == expectedGetWorkHistoriesRequest.GetUrl)))
            .ReturnsAsync(workHistoriesApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo((GetVolunteeringAndWorkExperiencesQueryResult)workHistoriesApiResponse);
    }
}