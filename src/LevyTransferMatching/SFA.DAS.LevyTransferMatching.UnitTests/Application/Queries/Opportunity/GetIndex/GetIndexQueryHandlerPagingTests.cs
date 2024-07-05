using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunity.GetIndex;

[TestFixture]
public class GetIndexQueryHandlerPagingTests
{
    private GetIndexQueryHandler _handler;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private GetIndexQuery _query;
    private GetPledgesResponse _pledges;
    private readonly Fixture _autoFixture = new ();

    [SetUp]
    public void SetUp()
    {
        _query = _autoFixture.Build<GetIndexQuery>()
            .Without(p => p.PageSize)
            .With(p => p.Page, 1)
            .Create();
        _pledges = _autoFixture.Build<GetPledgesResponse>()
            .With(p => p.Pledges, _autoFixture.CreateMany<GetPledgesResponse.Pledge>(50))
            .Create();

        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
        _levyTransferMatchingService.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_pledges);

        _handler = new GetIndexQueryHandler(_levyTransferMatchingService.Object, Mock.Of<IReferenceDataService>());
    }

    [Test]
    public async Task Handle_Returns_AllOpportunities()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Items.Select(x => x.Id).Should().BeEquivalentTo(_pledges.Pledges.Select(x => x.Id));
    }

    [Test]
    public async Task Handle_Returns_All50Opportunities()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.TotalItems.Should().Be(50);
        result.Items.Count.Should().Be(50);
    }

    [TestCase(1,20,20)]
    [TestCase(2,20,20)]
    [TestCase(3,20,10)]
    public async Task Handle_Returns_CorrectPageOfOpportunities(int page, int pageSize, int expectedRows)
    {

        _query.Page = page;
        _query.PageSize = pageSize;
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.TotalItems.Should().Be(50);
        result.Items.Count.Should().Be(expectedRows);
    }
}