using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetAccountsWithPledges;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Pledges
{
    [TestFixture]
    public class WhenGettingAccountIds
    {
        private PledgesController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetAccountsWithPledgesQueryResult _queryResult;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _queryResult = _fixture.Create<GetAccountsWithPledgesQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetAccountsWithPledgesQuery>(), CancellationToken.None))
                .ReturnsAsync(_queryResult);

            _controller = new PledgesController(_mediator.Object);
        }

        [Test]
        public async Task Then_Accounts_Are_Returned_Correctly()
        {
            var result = await _controller.GetAccountsWithPledges() as ObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as GetAccountsWithPledgesResponse;
            Assert.That(response, Is.Not.Null);

            CollectionAssert.AreEqual(_queryResult.AccountIds, response.AccountIds);
        }
    }
}