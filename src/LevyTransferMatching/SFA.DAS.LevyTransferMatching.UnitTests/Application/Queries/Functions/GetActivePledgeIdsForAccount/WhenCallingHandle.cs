using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions.GetActivePledgeIdsForAccount;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.GetActivePledgeIdsForAccount;

[TestFixture]
public class WhenCallingHandle
{
    private readonly Fixture _fixture = new();

    [Test]
    public async Task Then_Active_Pledge_Ids_Are_Returned()
    {
        // Arrange
        var query = new GetActivePledgeIdsForAccountQuery
        {
            AccountId = 123,
            Page = 2,
            PageSize = 50
        };

        var pledgesResponse = new GetPledgesResponse
        {
            Page = 2,
            TotalPages = 3,
            TotalPledges = 4,
            Pledges =
            [
                new GetPledgesResponse.Pledge { Id = 10 },
                new GetPledgesResponse.Pledge { Id = 11 }
            ]
        };

        var service = new Mock<ILevyTransferMatchingService>();
        service.Setup(x => x.GetPledges(It.Is<GetPledgesRequest>(r =>
                r.AccountId == query.AccountId &&
                r.GetUrl.Contains("pledgeStatusFilter=Active") &&
                r.GetUrl.Contains("page=2") &&
                r.GetUrl.Contains("pageSize=50"))))
            .ReturnsAsync(pledgesResponse);

        var handler = new GetActivePledgeIdsForAccountQueryHandler(service.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Page.Should().Be(2);
        result.TotalPages.Should().Be(3);
        result.TotalPledges.Should().Be(4);
        result.PledgeIds.ToArray().Should().Equal(10, 11);
    }

    [Test]
    public async Task Then_Empty_Result_Is_Returned_When_Service_Returns_Null()
    {
        // Arrange
        var query = _fixture.Create<GetActivePledgeIdsForAccountQuery>();

        var service = new Mock<ILevyTransferMatchingService>();
        service.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>()))
            .ReturnsAsync((GetPledgesResponse)null);

        var handler = new GetActivePledgeIdsForAccountQueryHandler(service.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.PledgeIds.Should().BeEmpty();
        result.Page.Should().Be(query.Page);
        result.TotalPages.Should().Be(0);
    }
}
