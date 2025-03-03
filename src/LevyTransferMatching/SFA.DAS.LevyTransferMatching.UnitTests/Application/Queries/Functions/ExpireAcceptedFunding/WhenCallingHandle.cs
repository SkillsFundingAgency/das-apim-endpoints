using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.ExpireAcceptedFunding;

[TestFixture]
public class WhenCallingHandle
{
    private ExpireAcceptedFundingCommandHandler _handler;
    private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
    private readonly Fixture _fixture = new();

    private ExpireAcceptedFundingCommand _command;
    private ExpireAcceptedFundingRequest _request;

    [SetUp]
    public void Setup()
    {
        _command = _fixture.Create<ExpireAcceptedFundingCommand>();

        var apiResponse = new ApiResponse<ExpireAcceptedFundingRequest>(
            new ExpireAcceptedFundingRequest(_command.ApplicationId,
            new ExpireAcceptedFundingRequestData())
            , HttpStatusCode.OK, string.Empty);

        _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

        _levyTransferMatchingService.Setup(x => x.ExpireAcceptedFunding(It.IsAny<ExpireAcceptedFundingRequest>()))
            .Callback<ExpireAcceptedFundingRequest>(r => _request = r)
            .ReturnsAsync(apiResponse);

        _handler = new ExpireAcceptedFundingCommandHandler(
            _levyTransferMatchingService.Object,
            Mock.Of<ILogger<ExpireAcceptedFundingCommandHandler>>());
    }

    [Test]
    public async Task Pledge_Is_Debited()
    {
        await _handler.Handle(_command, CancellationToken.None);

        var expireData = (ExpireAcceptedFundingRequestData)_request.Data;

        var expectedUrl = $"/applications/{_command.ApplicationId}/expire-accepted-funding";
        _request.PostUrl.Should().Be(expectedUrl);

        expireData.ApplicationId.Should().Be(_command.ApplicationId);
        expireData.UserId.Should().Be(string.Empty);
        expireData.UserDisplayName.Should().Be(string.Empty);
    }
}