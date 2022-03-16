using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Approvals.Queries;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Accounts
{
    [TestFixture]
    public class WhenGettingAccounts
    {
        private AccountsController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetAccountsWithCohortsQueryResult _queryResult;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _queryResult = _fixture.Create<GetAccountsWithCohortsQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetAccountsWithCohortsQuery>(), CancellationToken.None))
                .ReturnsAsync(_queryResult);

            _controller = new AccountsController(_mediator.Object, Mock.Of<ILogger<AccountsController>>());
        }

        [Test]
        public async Task Then_Accounts_Are_Returned_Correctly()
        {
            var result = await _controller.GetAccountsWithCohorts() as ObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value as GetAccountsWithCohortsResponse;
            Assert.IsNotNull(response);

            CollectionAssert.AreEqual(_queryResult.AccountIds, response.AccountIds);
        }
    }
}
