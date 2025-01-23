using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunity.GetIndex;

[TestFixture]
public class GetIndexQueryHandlerTests
{
    private GetIndexQueryHandler _handler;
    private Mock<IReferenceDataService> _referenceDataService;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private List<ReferenceDataItem> _sectors;
    private List<ReferenceDataItem> _jobRoles;
    private List<ReferenceDataItem> _levels;
    private GetIndexQuery _query;
    private GetPledgesResponse _pledges;
    private readonly Fixture _autoFixture = new Fixture();

    [SetUp]
    public void SetUp()
    {
        _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
        _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
        _levels = _autoFixture.Create<List<ReferenceDataItem>>();
        _query = _autoFixture.Build<GetIndexQuery>()
            .Without(p => p.PageSize)
            .With(p => p.Page, 1).Create();
        _pledges = _autoFixture.Create<GetPledgesResponse>();

        _referenceDataService = new Mock<IReferenceDataService>();
        _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);
        _referenceDataService.Setup(x => x.GetJobRoles()).ReturnsAsync(_jobRoles);
        _referenceDataService.Setup(x => x.GetLevels()).ReturnsAsync(_levels);

        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
        _levyTransferMatchingService.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_pledges);

        _handler = new GetIndexQueryHandler(_levyTransferMatchingService.Object, _referenceDataService.Object);
    }

    [Test]
    public async Task Handle_Returns_Opportunities()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Items.Select(x => x.Id).Should().BeEquivalentTo(_pledges.Pledges.Select(x => x.Id));
    }

    [Test]
    public async Task Handle_Returns_Sectors()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.Sectors.Should().BeEquivalentTo(_sectors);
    }

    [Test]
    public async Task Handle_Returns_JobRoles()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.JobRoles.Should().BeEquivalentTo(_jobRoles);
    }

    [Test]
    public async Task Handle_Returns_Levels()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.Levels.Should().BeEquivalentTo(_levels);
    }

    [Test]
    public async Task Handle_Does_Not_Return_Closed_Opportunities()
    {
        foreach (var pledge in _pledges.Pledges)
        {
            pledge.Status = PledgeStatus.Closed;
        }

        var result = await _handler.Handle(_query, CancellationToken.None);
        result.Items.Should().BeEmpty();
    }
}
