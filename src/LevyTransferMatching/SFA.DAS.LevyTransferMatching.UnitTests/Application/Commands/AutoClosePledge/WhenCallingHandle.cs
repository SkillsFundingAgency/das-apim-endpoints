using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.ClosePledgeRequest;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.AutoClosePledge
{
    public class WhenCallingHandle
    {       
        [Test]
        [MoqAutoData]
        public async Task Handle_ClosePledge_WhenPledgeBalance_Under_2000(
              [Frozen] Mock<ILevyTransferMatchingService> _levyTransferMatchingService,
              AutoClosePledgeCommand _command,
              ClosePledgeRequestData _apiRequestData,
              AutoClosePledgeCommandHandler _handler
              )
        {
            // Arrange
            var applicationResponse = new GetApplicationResponse { Amount = 1000 };
            var pledgeResponse = new Pledge { RemainingAmount = 2500 };
            var _closePledgeResponse = new ApiResponse<ClosePledgeRequest>(null, HttpStatusCode.OK, null);
            
            _levyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(_command.ApplicationId.ToString()))))
                .ReturnsAsync(applicationResponse);

            _levyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == _command.PledgeId)))
                .ReturnsAsync(pledgeResponse);

            _levyTransferMatchingService.Setup(x => x.ClosePledge(It.Is<ClosePledgeRequest>(request =>
                 request._pledgeId == _command.PledgeId
                 && request.Data == _apiRequestData
             )))
                .ReturnsAsync(_closePledgeResponse);

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            Assert.AreEqual(true, result.PledgeClosed);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_Returns_NotClosed__WhenPledgeBalance_Over_2000(
            [Frozen] Mock<ILevyTransferMatchingService> _levyTransferMatchingService,
              AutoClosePledgeCommand _command,
              AutoClosePledgeCommandHandler _handler)
        {
            // Arrange
            var applicationResponse = new GetApplicationResponse { Amount = 1000 }; 
            var pledgeResponse = new Pledge { RemainingAmount = 5000 };

            _levyTransferMatchingService
                 .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(_command.ApplicationId.ToString()))))
                 .ReturnsAsync(applicationResponse);

            _levyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == _command.PledgeId)))
                .ReturnsAsync(pledgeResponse);

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            Assert.AreEqual(false, result.PledgeClosed);
        }
        
    }
}