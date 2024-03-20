using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Commands.StopApprenticeship;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingManageApprenticeshipDetails
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private GetManageApprenticeshipDetailsQueryResult _queryResult;
        private StopApprenticeshipRequest _stopRequest;
        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetManageApprenticeshipDetailsQueryResult>();

            _apprenticeshipId = fixture.Create<long>();
            _stopRequest = fixture.Create<StopApprenticeshipRequest>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetManageApprenticeshipDetailsQuery>(q =>
                        q.ApprenticeshipId == _apprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _mediator.Setup(x => x.Send(It.Is<StopApprenticeshipCommand>(q =>
                        q.ApprenticeshipId == _apprenticeshipId
                         && q.MadeRedundant == _stopRequest.MadeRedundant
                    && q.StopDate == _stopRequest.StopDate
                   && q.UserInfo == _stopRequest.UserInfo),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new Unit());

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, mapper);
        }

        [Test]
        public async Task GetManageApprenticeshipDetailsResponseIsReturned()
        {
            var result = await _controller.ManageApprenticeshipDetails( _apprenticeshipId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult)result;
            Assert.That(okObjectResult.Value, Is.InstanceOf<GetManageApprenticeshipDetailsResponse>());
            var objectResult = (GetManageApprenticeshipDetailsResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.That(comparisonResult.AreEqual, Is.True);
        } 
        
        [Test]
        public async Task StopApprenticeshipCommand_IsSent()
        {
            var result = await _controller.StopApprenticeship( _apprenticeshipId, _stopRequest);

            Assert.That(result, Is.InstanceOf<OkResult>());
            _mediator.Verify(m => m.Send(It.IsAny<StopApprenticeshipCommand>(), default), Times.Once);
        }
    }
}
