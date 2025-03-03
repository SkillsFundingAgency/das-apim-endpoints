using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;

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
            response.TotalItems.Should().Be(_queryResult.TotalPledges);
            response.TotalPages.Should().Be(_queryResult.TotalPages);
            response.Page.Should().Be(_queryResult.Page);
            response.PageSize.Should().Be(_queryResult.PageSize);

            response.Items.Should().NotBeNull();
            response.Items.Should().NotBeEmpty(); response.StartingTransferAllowance.Should().Be(_queryResult.StartingTransferAllowance);
            response.Items.Should().NotBeEmpty();
            response.Items.Any(x => x.Id == 0).Should().BeFalse();
            response.Items.Any(x => x.Amount == 0).Should().BeFalse();
            response.Items.Any(x => x.RemainingAmount == 0).Should().BeFalse();
            response.Items.Any(x => x.ApplicationCount == 0).Should().BeFalse();
            response.Items.Any(x => x.Status == string.Empty).Should().BeFalse();
            response.CurrentYearEstimatedCommittedSpend.Should().Be(_queryResult.CurrentYearEstimatedCommittedSpend);
        }
    }
}