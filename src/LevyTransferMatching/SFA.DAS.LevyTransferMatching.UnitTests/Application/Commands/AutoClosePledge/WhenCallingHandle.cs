using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.AutoClosePledge
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private AutoClosePledgeCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<ILogger<AutoClosePledgeCommandHandler>> _loggerMock;
        private AutoClosePledgeCommand _command;
        private ApiResponse<ClosePledgeRequest> _closePledgeResponse;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _loggerMock = new Mock<ILogger<AutoClosePledgeCommandHandler>>();
            _handler = new AutoClosePledgeCommandHandler(_levyTransferMatchingService.Object, _loggerMock.Object);
            _command = _fixture.Create<AutoClosePledgeCommand>();
        }
        
        [Test]
        public async Task Handle_ClosePledge_WhenPledgeBalance_Under_2000()
        {
            // Arrange
            var applicationResponse = new GetApplicationResponse { Amount = 1000 };
            var pledgeResponse = new Pledge { RemainingAmount = 2500 };    
            _closePledgeResponse = new ApiResponse<ClosePledgeRequest>(null, HttpStatusCode.OK, null);

            _levyTransferMatchingService.Setup(x => x.GetApplication(It.IsAny<GetApplicationRequest>()))
                .ReturnsAsync(applicationResponse);

            _levyTransferMatchingService.Setup(x => x.GetPledge(It.IsAny<int>()))
                .ReturnsAsync(pledgeResponse);

            _levyTransferMatchingService.Setup(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()))
                .ReturnsAsync(_closePledgeResponse);
            
            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            Assert.AreEqual(true, result.PledgeClosed);
        }

        [Test]
        public async Task Handle_Returns_NotClosed__WhenPledgeBalance_Over_2000()
        {
            // Arrange
            var applicationResponse = new GetApplicationResponse { Amount = 1000 }; 
            var pledgeResponse = new Pledge { RemainingAmount = 5000 };  

            _levyTransferMatchingService.Setup(x => x.GetApplication(It.IsAny<GetApplicationRequest>()))
                .ReturnsAsync(applicationResponse);

            _levyTransferMatchingService.Setup(x => x.GetPledge(It.IsAny<int>()))
                .ReturnsAsync(pledgeResponse);
            
            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            Assert.AreEqual(false, result.PledgeClosed);
        }
        
    }
}