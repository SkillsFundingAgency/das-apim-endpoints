using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperiences;
using SFA.DAS.FindAnApprenticeship.Domain;
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
        GetApplicationApiResponse applicationApiResponse,
        GetWorkHistoriesApiResponse workHistoriesApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetVolunteeringAndWorkExperiencesQueryHandler handler)
    {
        var expectedGetWorkHistoriesRequest = new GetWorkHistoriesApiRequest(query.ApplicationId, query.CandidateId, WorkHistoryType.WorkExperience);
        candidateApiClient
            .Setup(client => client.Get<GetWorkHistoriesApiResponse>(
                It.Is<GetWorkHistoriesApiRequest>(r => r.GetUrl == expectedGetWorkHistoriesRequest.GetUrl)))
            .ReturnsAsync(workHistoriesApiResponse);

        var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);

        candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(new GetVolunteeringAndWorkExperiencesQueryResult
        {
            VolunteeringAndWorkExperiences = workHistoriesApiResponse.WorkHistories.Select(x => (GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience)x).ToList()
        });
    }
}