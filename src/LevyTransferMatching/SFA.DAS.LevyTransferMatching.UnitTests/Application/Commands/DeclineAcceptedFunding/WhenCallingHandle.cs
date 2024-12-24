using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineAcceptedFunding;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DeclineAcceptedFunding;

[TestFixture]
public class WhenCallingHandle
{
    private DeclineAcceptedFundingCommandHandler _handler;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private readonly Fixture _fixture = new();

    private DeclineAcceptedFundingCommand _command;
    private DeclineAcceptedFundingRequest _request;

    [SetUp]
    public void Setup()
    {
        _command = _fixture.Create<DeclineAcceptedFundingCommand>();

        var apiResponse = new ApiResponse<DeclineAcceptedFundingRequest>(
            new DeclineAcceptedFundingRequest(_command.ApplicationId, 
            new DeclineAcceptedFundingRequestData())
            , HttpStatusCode.OK, string.Empty);

        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

        _levyTransferMatchingService.Setup(x => x.DeclineAcceptedFunding(It.IsAny<DeclineAcceptedFundingRequest>()))
            .Callback<DeclineAcceptedFundingRequest>(r => _request = r)
            .ReturnsAsync(apiResponse);

        _handler = new DeclineAcceptedFundingCommandHandler(
            _levyTransferMatchingService.Object, 
            Mock.Of<ILogger<DeclineAcceptedFundingCommandHandler>>());
    }

    [Test]
    public async Task Pledge_Is_Debited()
    {
        await _handler.Handle(_command, CancellationToken.None);

        var declineData = (DeclineAcceptedFundingRequestData)_request.Data;

        var expectedUrl = $"/applications/{_command.ApplicationId}/decline-accepted-funding";
        _request.PostUrl.Should().Be(expectedUrl);

        declineData.ApplicationId.Should().Be(_command.ApplicationId);
        declineData.UserId.Should().Be(string.Empty);
        declineData.UserDisplayName.Should().Be(string.Empty);
    }
}
