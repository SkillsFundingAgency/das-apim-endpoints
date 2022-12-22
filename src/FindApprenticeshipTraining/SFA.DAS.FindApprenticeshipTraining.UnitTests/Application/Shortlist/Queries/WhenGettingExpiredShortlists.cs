using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Queries
{
    public class WhenGettingExpiredShortlists
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Made_And_Ids_Returned_In_The_Response(
            GetExpiredShortlistsQuery query,
            GetExpiredShortlistsResponse apiResponse,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> shortlistApiClient,
            GetExpiredShortlistsQueryHandler handler)
        {
            //Arrange
            shortlistApiClient.Setup(x =>
                x.Get<GetExpiredShortlistsResponse>(It.Is<GetExpiredShortlistsRequest>(c =>
                    c.GetUrl.Contains($"expired?expiryInDays={query.ExpiryInDays}")))).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.UserIds.Should().BeEquivalentTo(apiResponse.UserIds);
        }
    }
}