using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingEditApprenticeship
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private GetEditApprenticeshipQueryResult _queryResult;

        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetEditApprenticeshipQueryResult>();

            _apprenticeshipId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetEditApprenticeshipQuery>(q =>
                        q.ApprenticeshipId == _apprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, Mock.Of<IMapper>());
        }

        [Test]
        public async Task GetEditApprenticeshipResponseIsReturned()
        {
            var result = await _controller.EditApprenticeship( _apprenticeshipId);

            result.Should().BeOfType<OkObjectResult>();

            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().BeOfType<GetEditApprenticeshipResponse>();

            var value = (GetEditApprenticeshipResponse)okObjectResult.Value
            value.CourseName.Should().Be(_queryResult.CourseName);
            value.HasMultipleDeliveryModelOptions.Should().Be(_queryResult.HasMultipleDeliveryModelOptions);
            value.IsFundedByTransfer.Should().Be(_queryResult.IsFundedByTransfer);
            value.LearningType.Should().Be(_queryResult.LearningType);
        }
    }
}
