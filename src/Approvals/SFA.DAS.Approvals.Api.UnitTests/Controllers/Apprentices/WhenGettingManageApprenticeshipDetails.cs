using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.Exceptions;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

[TestFixture]
public class WhenGettingManageApprenticeshipDetails
{
    private ApprenticesController _controller;
    private Mock<IMediator> _mediator;
    private GetManageApprenticeshipDetailsQueryResult _queryResult;

    private long _apprenticeshipId;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _queryResult = fixture.Create<GetManageApprenticeshipDetailsQueryResult>();

        _apprenticeshipId = fixture.Create<long>();

        _mediator = new Mock<IMediator>();
        _mediator.Setup(x => x.Send(It.Is<GetManageApprenticeshipDetailsQuery>(q =>
                    q.ApprenticeshipId == _apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_queryResult);

        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, mapper);
    }

    [Test]
    public async Task GetManageApprenticeshipDetailsResponseIsReturned()
    {
        var result = await _controller.ManageApprenticeshipDetails(_apprenticeshipId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = (OkObjectResult)result;

        okObjectResult.Value.Should().BeOfType<GetManageApprenticeshipDetailsResponse>();

        var objectResult = (GetManageApprenticeshipDetailsResponse)okObjectResult.Value;

        var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

        var comparisonResult = compare.Compare(_queryResult, objectResult);

        comparisonResult.AreEqual.Should().BeTrue();
    }

    [Test]
    public async Task NotFoundIsReturnedWhenApprenticeshipNotFound()
    {
        _mediator.Setup(x => x.Send(It.Is<GetManageApprenticeshipDetailsQuery>(q =>
                    q.ApprenticeshipId == _apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .Throws<ResourceNotFoundException>();

        var result = await _controller.ManageApprenticeshipDetails(_apprenticeshipId);

        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Test]
    public async Task UnauthorizedIsReturnedWhenNotAuthorized()
    {
        _mediator.Setup(x => x.Send(It.Is<GetManageApprenticeshipDetailsQuery>(q =>
                    q.ApprenticeshipId == _apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .Throws<UnauthorizedAccessException>();

        var result = await _controller.ManageApprenticeshipDetails(_apprenticeshipId);

        result.Should().BeOfType<UnauthorizedResult>();
    }
}