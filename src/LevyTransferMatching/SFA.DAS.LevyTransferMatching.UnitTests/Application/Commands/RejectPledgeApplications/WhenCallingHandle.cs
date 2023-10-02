using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectPledgeApplications
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private RejectPledgeApplicationsCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<ILogger<RejectPledgeApplicationsCommandHandler>> _loggerMock;
        private RejectPledgeApplicationsCommand _command;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _loggerMock = new Mock<ILogger<RejectPledgeApplicationsCommandHandler>>();
            _handler = new RejectPledgeApplicationsCommandHandler(_levyTransferMatchingService.Object, _loggerMock.Object);
            _command = _fixture.Create<RejectPledgeApplicationsCommand>();
        }
        
        [Test]
        public async Task Handle_RejectsApplications()
        {
            // Arrange
            var applicationsResponse = new GetApplicationsResponse
            {
                Applications = new List<GetApplicationsResponse.Application> 
                {
                    new GetApplicationsResponse.Application { Id = 1 },
                    new GetApplicationsResponse.Application { Id = 2 }
                }
            };

            _levyTransferMatchingService.Setup(x => 
                    x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(applicationsResponse);

            _levyTransferMatchingService.Setup(x => 
                    x.RejectApplication(It.IsAny<RejectApplicationRequest>()))
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            _levyTransferMatchingService.Verify(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()), Times.Once);

            foreach (var application in applicationsResponse.Applications)
            {
                _levyTransferMatchingService.Verify(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()));
            }

            Assert.AreEqual(Unit.Value, result);
        }
    }
}