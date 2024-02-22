using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetVolunteering;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetVolunteeringOrWorkExperienceItemQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetVolunteeringOrWorkExperienceItemQuery query,
        GetWorkHistoryItemApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetVolunteeringOrWorkExperienceItemQueryHandler handler)
    {
        var expectedGetDeleteJobRequest = new GetWorkHistoryItemApiRequest(query.ApplicationId, query.CandidateId, query.Id, Models.WorkHistoryType.WorkExperience);
        candidateApiClient
            .Setup(client => client.Get<GetWorkHistoryItemApiResponse>(
                It.Is<GetWorkHistoryItemApiRequest>(r => r.GetUrl == expectedGetDeleteJobRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo((GetVolunteeringOrWorkExperienceItemQueryResult)apiResponse);
    }
}
