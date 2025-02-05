using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetApplications
{
    [TestFixture]
    public class GetApplicationsQueryHandlerTests
    {
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingServiceMock;
        private GetApplicationsQueryHandler _handler;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _levyTransferMatchingServiceMock = new Mock<ILevyTransferMatchingService>();
            _handler = new GetApplicationsQueryHandler(_levyTransferMatchingServiceMock.Object);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Should_Return_Correct_Result()
        {
            // Arrange
            var request = _fixture.Create<GetApplicationsQuery>();
            var applicationsResponse = _fixture.Create<GetApplicationsResponse>();
            var pledgeResponse = _fixture.Create<Pledge>();

            _levyTransferMatchingServiceMock
                .Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(applicationsResponse);

            _levyTransferMatchingServiceMock
                .Setup(x => x.GetPledge(It.IsAny<int>()))
                .ReturnsAsync(pledgeResponse);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.TotalItems.Should().Be(applicationsResponse.Applications.Count());
            result.PageSize.Should().Be(request.PageSize ?? int.MaxValue);
            result.Page.Should().Be(request.Page);
            result.PledgeStatus.Should().Be(pledgeResponse.Status);
            result.TotalAmount.Should().Be(pledgeResponse.Amount);
            result.RemainingAmount.Should().Be(pledgeResponse.RemainingAmount);
            result.AutomaticApprovalOption.Should().Be(pledgeResponse.AutomaticApprovalOption);
        }

        [Test]
        public async Task Handle_Should_Call_GetApplications_And_GetPledge()
        {
            // Arrange
            var request = _fixture.Create<GetApplicationsQuery>();
            var applicationsResponse = _fixture.Create<GetApplicationsResponse>();
            var pledgeResponse = _fixture.Create<Pledge>();

            _levyTransferMatchingServiceMock
                .Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(applicationsResponse);

            _levyTransferMatchingServiceMock
                .Setup(x => x.GetPledge(It.IsAny<int>()))
                .ReturnsAsync(pledgeResponse);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _levyTransferMatchingServiceMock.Verify(x => x.GetApplications(It.Is<GetApplicationsRequest>(r =>
                r.PledgeId == request.PledgeId &&
                r.SortOrder == request.SortOrder &&
                r.SortDirection == request.SortDirection)), Times.Once);

            _levyTransferMatchingServiceMock.Verify(x => x.GetPledge(request.PledgeId), Times.Once);
        }
    }
}
