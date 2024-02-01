using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectPledgeApplications
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private RejectPledgeApplicationsCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private RejectPledgeApplicationsCommand _command;
        private RejectApplicationRequest _rejectApplicationRequest;
        private GetApplicationsResponse _getApplicationsResponse;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<RejectPledgeApplicationsCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _getApplicationsResponse = _fixture.Create<GetApplicationsResponse>();

            _levyTransferMatchingService.Setup(x => 
                x.GetApplications(It.Is<GetApplicationsRequest>(r => 
                    r.PledgeId == _command.PledgeId 
                    && r.ApplicationStatusFilter == ApplicationStatus.Pending)))
               .ReturnsAsync(_getApplicationsResponse);

            _levyTransferMatchingService.Setup(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()))
                .Callback<RejectApplicationRequest>(request => _rejectApplicationRequest = request)
                .Returns(Task.CompletedTask);

            _handler = new RejectPledgeApplicationsCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<RejectPledgeApplicationsCommandHandler>>());
        }

        [Test]
        public async Task Each_Pending_Application_Is_Rejected()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _levyTransferMatchingService.Verify(
                x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()),
                Times.Exactly(_getApplicationsResponse.Applications.Count()));
        }
    }
}
