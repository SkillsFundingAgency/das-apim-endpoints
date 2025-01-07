using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineApprovedFunding;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DeclineApprovedFunding;

[TestFixture]
public class WhenCallingHandle
{
    private DeclineApprovedFundingCommandHandler _handler;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private readonly Fixture _fixture = new();

    private DeclineApprovedFundingCommand _command;
    private DeclineApprovedFundingRequest _request;

    [SetUp]
    public void Setup()
    {
        _command = _fixture.Create<DeclineApprovedFundingCommand>();

        var apiResponse = new ApiResponse<DeclineApprovedFundingRequest>(
            new DeclineApprovedFundingRequest(_command.ApplicationId,
            new DeclineApprovedFundingRequestData())
            , HttpStatusCode.OK, string.Empty);

        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

        _levyTransferMatchingService.Setup(x => x.DeclineApprovedFunding(It.IsAny<DeclineApprovedFundingRequest>()))
            .Callback<DeclineApprovedFundingRequest>(r => _request = r)
            .ReturnsAsync(apiResponse);

        _handler = new DeclineApprovedFundingCommandHandler(
            _levyTransferMatchingService.Object,
            Mock.Of<ILogger<DeclineApprovedFundingCommandHandler>>());
    }

    [Test]
    public async Task Pledge_Is_Debited()
    {
        await _handler.Handle(_command, CancellationToken.None);

        var declineData = (DeclineApprovedFundingRequestData)_request.Data;

        var expectedUrl = $"/applications/{_command.ApplicationId}/decline-approved-funding";
        _request.PostUrl.Should().Be(expectedUrl);

        declineData.ApplicationId.Should().Be(_command.ApplicationId);
        declineData.UserId.Should().Be(string.Empty);
        declineData.UserDisplayName.Should().Be(string.Empty);
    }
}
