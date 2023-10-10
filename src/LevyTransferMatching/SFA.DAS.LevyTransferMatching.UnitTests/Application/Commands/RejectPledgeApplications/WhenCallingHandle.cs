using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectPledgeApplications
{
    public class WhenCallingHandle
    {      
        [Test]
        [MoqAutoData]
        public async Task Handle_RejectsApplications(
            [Frozen] Mock<ILevyTransferMatchingService> _levyTransferMatchingService,
            RejectPledgeApplicationsCommand _command,
            GetApplicationsResponse applicationsResponse,
            RejectPledgeApplicationsCommandHandler _handler
            )
        {  
            _levyTransferMatchingService.Setup(x => 
                    x.GetApplications(It.Is<GetApplicationsRequest>(request => 
                    request.PledgeId == _command.PledgeId)))
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
                _levyTransferMatchingService.Verify(x => x.RejectApplication(It.Is<RejectApplicationRequest>(
                    request => request.ApplicationId == application.Id)));
            }

            Assert.AreEqual(Unit.Value, result);
        }
    }
}