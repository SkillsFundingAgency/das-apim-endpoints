using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.ApplicationsForAutomaticExpire;

[TestFixture]
public class WhenCallingHandle
{
    private ApplicationsForAutomaticExpireQueryHandler _handler;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private GetApplicationsToAutoExpireResponse _applicationsToExpireApiResponse;
    private readonly Fixture _autoFixture = new();

    [SetUp]
    public void Setup()
    {
        _applicationsToExpireApiResponse = _autoFixture.Create<GetApplicationsToAutoExpireResponse>();
        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
        _levyTransferMatchingService.Setup(x =>
                x.GetApplicationsToAutoExpire(It.IsAny<GetApplicationsToAutoExpireRequest>()))
                .ReturnsAsync(_applicationsToExpireApiResponse);

        _handler = new ApplicationsForAutomaticExpireQueryHandler(_levyTransferMatchingService.Object);
    }

    [Test]
    public async Task Handle_Returns_Levels()
    {
        var result = await _handler.Handle(new ApplicationsForAutomaticExpireQuery(), CancellationToken.None);
        result.ApplicationIdsToExpire.Should().BeEquivalentTo(_applicationsToExpireApiResponse.ApplicationIdsToExpire);
    }
}