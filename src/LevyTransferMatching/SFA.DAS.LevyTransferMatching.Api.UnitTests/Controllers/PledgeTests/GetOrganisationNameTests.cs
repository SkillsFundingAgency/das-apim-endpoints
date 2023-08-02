using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetOrganisationName;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class GetOrganisationNameTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetOrganisationNameQueryResult _queryResult;
        private string _encodedAccountId;


        [SetUp]
        public void SetUp()
        {
            _encodedAccountId = _fixture.Create<string>();

            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetOrganisationNameQueryResult>();
            _mediator.Setup(x => x.Send(It.Is<GetOrganisationNameQuery>(q => q.EncodedAccountId == _encodedAccountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }


        [Test]
        public async Task GetOrganisationName_Returns_GetOrganisationNameResponse()
        {
            var controllerResponse = await _controller.Organisation(_encodedAccountId);

            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var response = okObjectResult.Value as GetOrganisationNameResponse;
            Assert.IsNotNull(response);

            Assert.AreEqual(_queryResult.DasAccountName, response.DasAccountName);

        }

    }
}
