using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetEqualityQuestions;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    [TestFixture]
    public class WhenHandlingGetEqualityQuestionsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_EqualityQuestions_From_Candidates_Api(
            GetEqualityQuestionsQuery query,
            GetAboutYouItemApiResponse apiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            GetEqualityQuestionsQueryHandler handler)
        {
            var expectedUri = new GetAboutYouItemApiRequest(query.CandidateId).GetUrl;
            mockApiClient
                .Setup(client => client.Get<GetAboutYouItemApiResponse>(
                    It.Is<GetAboutYouItemApiRequest>(c => c.GetUrl == expectedUri)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(apiResponse.AboutYou, config => config.Excluding(x => x.Id));
        }
    }
}
