using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchResults;
using FluentAssertions;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingGetSearchResultsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Count_Returned(
            GetApprenticeshipCountResponse mockApiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> mockApiClient,
            [Greedy] SearchResultsQueryHandler sut)
        {
            mockApiClient.Setup(x => x.Get<GetApprenticeshipCountResponse>(It.IsAny<GetApprenticeshipCountRequest>())).ReturnsAsync(mockApiResponse);

            var actual = await sut.Handle(new SearchResultsQuery(), CancellationToken.None);

            actual.TotalApprenticeshipCount.Should().Be(mockApiResponse.TotalVacancies);
        }
    }
}
