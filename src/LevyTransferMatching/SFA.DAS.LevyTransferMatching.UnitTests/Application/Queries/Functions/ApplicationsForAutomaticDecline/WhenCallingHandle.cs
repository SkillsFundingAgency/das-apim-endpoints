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

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.ApplicationsForAutomaticDecline;

[TestFixture]
public class WhenCallingHandle
{
    private ApplicationsForAutomaticDeclineQueryHandler _handler;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private GetApplicationsToAutoDeclineResponse _applicationsToDeclineApiResponse;
    private readonly Fixture _autoFixture = new();

    [SetUp]
    public void Setup()
    {
        _applicationsToDeclineApiResponse = _autoFixture.Create<GetApplicationsToAutoDeclineResponse>();
        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
        _levyTransferMatchingService.Setup(x =>
                x.GetApplicationsToAutoDecline(It.IsAny<GetApplicationsToAutoDeclineRequest>()))
                .ReturnsAsync(_applicationsToDeclineApiResponse);

        _handler = new ApplicationsForAutomaticDeclineQueryHandler(_levyTransferMatchingService.Object);
    }

    [Test]
    public async Task Handle_Returns_Levels()
    {
        var result = await _handler.Handle(new ApplicationsForAutomaticDeclineQuery(), CancellationToken.None);
        result.ApplicationIdsToDecline.Should().BeEquivalentTo(_applicationsToDeclineApiResponse.ApplicationIdsToDecline);

    }
}
