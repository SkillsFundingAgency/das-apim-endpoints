using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetInactiveCandidates;
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
            GetInactiveCandidatesQuery query,
            GetInactiveCandidatesApiResponse mockCandidatesByActivityApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            GetInactiveCandidatesQueryHandler handler)
        {
            var expectedGetCandidatesByActivityApiRequest =
                new GetInactiveCandidatesApiRequest(query.CutOffDateTime.ToString("O"), query.PageNumber, query.PageSize);

            mockCandidateApiClient
                .Setup(client => client.Get<GetInactiveCandidatesApiResponse>(
                    It.Is<GetInactiveCandidatesApiRequest>(c =>
                        c.GetUrl == expectedGetCandidatesByActivityApiRequest.GetUrl)))
                .ReturnsAsync(mockCandidatesByActivityApiResponse);


            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Candidates.Should().BeEquivalentTo(mockCandidatesByActivityApiResponse.Candidates);
            actual.PageSize.Should().Be(mockCandidatesByActivityApiResponse.PageSize);
            actual.PageIndex.Should().Be(mockCandidatesByActivityApiResponse.PageIndex);
            actual.TotalPages.Should().Be(mockCandidatesByActivityApiResponse.TotalPages);
            actual.TotalCount.Should().Be(mockCandidatesByActivityApiResponse.TotalCount);
        }
    }
}
