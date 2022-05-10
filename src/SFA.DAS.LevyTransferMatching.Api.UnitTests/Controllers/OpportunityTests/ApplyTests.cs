using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunities;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    [TestFixture]
    public class ApplyTests
    {
        private OpportunityController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();

        private int _opportunityId;
        private long _accountId;
        private ApplyRequest _request;
        private CreateApplicationCommandResult _result;

        [SetUp]
        public void SetUp()
        {
            _opportunityId = _fixture.Create<int>();
            _accountId = _fixture.Create<long>();
            _request = _fixture.Create<ApplyRequest>();
            _result = _fixture.Create<CreateApplicationCommandResult>();

            _mediator = new Mock<IMediator>();

            _controller = new OpportunityController(Mock.Of<ILogger<OpportunityController>>(),_mediator.Object);

            _mediator.Setup(x => x.Send(It.Is<CreateApplicationCommand>(command =>
                    command.PledgeId == _opportunityId &&
                    command.EmployerAccountId == _accountId &&
                    command.EncodedAccountId == _request.EncodedAccountId &&
                    command.Details == _request.Details &&
                    command.StandardId == _request.StandardId &&
                    command.NumberOfApprentices == _request.NumberOfApprentices &&
                    command.StartDate == _request.StartDate &&
                    command.HasTrainingProvider == _request.HasTrainingProvider &&
                    command.Sectors.Equals(_request.Sectors) &&
                    command.Locations.Equals(_request.Locations) &&
                    command.AdditionalLocation == _request.AdditionalLocation &&
                    command.SpecificLocation == _request.SpecificLocation &&
                    command.FirstName == _request.FirstName &&
                    command.LastName == _request.LastName &&
                    command.EmailAddresses.Equals(_request.EmailAddresses) &&
                    command.BusinessWebsite == _request.BusinessWebsite &&
                    command.UserId == _request.UserId &&
                    command.UserDisplayName == _request.UserDisplayName
                ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_result);
        }

        [Test]
        public async Task Post_Returns_ApplicationId()
        {
            var actionResult = await _controller.Apply(_accountId, _opportunityId, _request);
            var createdResult = actionResult as CreatedResult;
            Assert.IsNotNull(createdResult);
            var response = createdResult.Value as ApplyResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_result.ApplicationId, response.ApplicationId);
        }
    }
}
