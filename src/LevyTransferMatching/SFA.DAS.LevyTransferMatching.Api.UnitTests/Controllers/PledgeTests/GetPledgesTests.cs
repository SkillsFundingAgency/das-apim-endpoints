using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class GetPledgesTests
    {
        private PledgeController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetPledgesQueryResult _queryResult;
        private int _accountId;
        private int _page;
        private int? _pageSize;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _queryResult = _fixture.Create<GetPledgesQueryResult>();
            _accountId = _fixture.Create<int>();
            _pageSize = _fixture.Create<int>();
            _page = _fixture.Create<int>();
            _mediator.Setup(x => x.Send(It.IsAny<GetPledgesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task GetPledges_Returns_GetPledgesResponse()
        {
            var controllerResponse = await _controller.Pledges(_accountId);

            var okObjectResult = controllerResponse as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            var response = okObjectResult.Value as GetPledgesResponse;
            response.Should().NotBeNull();
            response.TotalPledges.Should().Be(_queryResult.TotalPledges);
            response.TotalPages.Should().Be(_queryResult.TotalPages);
            response.Page.Should().Be(_queryResult.Page);
            response.PageSize.Should().Be(_queryResult.PageSize);

            response.Pledges.Should().NotBeNull();
            response.Pledges.Should().NotBeEmpty();
            response.Pledges.Any(x => x.Id == 0).Should().BeFalse();
            response.Pledges.Any(x => x.Amount == 0).Should().BeFalse();
            response.Pledges.Any(x => x.RemainingAmount == 0).Should().BeFalse();
            response.Pledges.Any(x => x.ApplicationCount == 0).Should().BeFalse();
            response.Pledges.Any(x => x.Status == string.Empty).Should().BeFalse();
        }

        [Test]
        public async Task GetPledges_Passes_Parameter_To_Query()
        {
            await _controller.Pledges(_accountId, _page, _pageSize);

            _mediator.Verify(x=>x.Send(It.Is<GetPledgesQuery>(p=>p.AccountId == _accountId && p.Page == _page && p.PageSize == _pageSize), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GetPledges_Sets_Page_to_1_When_its_null_and_passes_To_Query()
        {
            await _controller.Pledges(_accountId, null, null);

            _mediator.Verify(x => x.Send(It.Is<GetPledgesQuery>(p => p.Page == 1 && p.PageSize == null), It.IsAny<CancellationToken>()));
        }
    }
}