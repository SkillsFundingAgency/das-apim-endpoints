using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Candidates
{
    [TestFixture]
    public class WhenHandlingGetCandidatesByActivityQuery
    {
        [Test]
        [MoqAutoData]
        public async Task Then_The_Candidates_Are_Returned(
            GetCandidateByActivityQuery query,
            GetCandidatesByActivityApiResponse mockCandidatesByActivityApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            GetCandidateByActivityQueryHandler handler)
        {
            var expectedGetCandidatesByActivityApiRequest =
                new GetCandidatesByActivityApiRequest(query.CutOffDateTime);

            mockCandidateApiClient
                .Setup(client => client.Get<GetCandidatesByActivityApiResponse>(
                    It.Is<GetCandidatesByActivityApiRequest>(c =>
                        c.GetUrl == expectedGetCandidatesByActivityApiRequest.GetUrl)))
                .ReturnsAsync(mockCandidatesByActivityApiResponse);


            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Candidates.Should().BeEquivalentTo(mockCandidatesByActivityApiResponse.Candidates);
        }
    }
}
