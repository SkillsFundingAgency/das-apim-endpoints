using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetDeleteVolunteeringOrWorkExperienceQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetDeleteVolunteeringOrWorkExperienceQuery query,
        GetDeleteVolunteeringOrWorkExperienceApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetDeleteVolunteeringOrWorkExperienceQueryHandler handler)
    {
        var expectedGetDeleteJobRequest = new GetDeleteVolunteeringOrWorkExperienceApiRequest(query.ApplicationId, query.CandidateId, query.Id);
        candidateApiClient
            .Setup(client => client.Get<GetDeleteVolunteeringOrWorkExperienceApiResponse>(
                It.Is<GetDeleteVolunteeringOrWorkExperienceApiRequest>(r => r.GetUrl == expectedGetDeleteJobRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo((GetDeleteVolunteeringOrWorkExperienceQueryResult)apiResponse);
    }
}
