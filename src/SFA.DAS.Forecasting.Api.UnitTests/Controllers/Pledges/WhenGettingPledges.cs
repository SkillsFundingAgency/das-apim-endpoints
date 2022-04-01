using System.Linq;
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
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Pledges
{
    [TestFixture]
    public class WhenGettingPledges
    {
        private PledgesController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetPledgesQueryResult _queryResult;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _queryResult = _fixture.Create<GetPledgesQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetPledgesQuery>(), CancellationToken.None))
                .ReturnsAsync(_queryResult);

            _controller = new PledgesController(_mediator.Object, Mock.Of<ILogger<PledgesController>>());
        }

        [Test]
        public async Task Then_Pledges_Are_Returned_Correctly()
        {
            var result = await _controller.GetPledges(0, 100) as ObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value as GetPledgesResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_queryResult.Page, response.Page);
            Assert.AreEqual(_queryResult.PageSize, response.PageSize);
            Assert.AreEqual(_queryResult.TotalPages, response.TotalPages);
            Assert.AreEqual(_queryResult.TotalPledges, response.TotalPledges);

            Assert.AreEqual(_queryResult.Pledges.Count(), response.Pledges.Count());

            var i = 0;

            foreach (var pledge in response.Pledges)
            {
                var expected = _queryResult.Pledges.ToArray()[i];
                Assert.AreEqual(expected.Id, pledge.Id);
                Assert.AreEqual(expected.AccountId, pledge.AccountId);
                i++;
            }
        }

        [Test]
        public async Task Then_Paging_Options_Are_Honoured()
        {
            var page = _fixture.Create<int>();
            var pageSize = _fixture.Create<int>();

            await _controller.GetPledges(page, pageSize);

            _mediator.Verify(x =>
                x.Send(It.Is<GetPledgesQuery>(q => q.Page == page && q.PageSize == pageSize),
                    It.IsAny<CancellationToken>()));
        }
    }
}
