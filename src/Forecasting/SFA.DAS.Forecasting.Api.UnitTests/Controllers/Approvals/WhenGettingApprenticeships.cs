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
using SFA.DAS.Forecasting.Api.UnitTests.Helpers;
using SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Approvals
{
    [TestFixture]
    public class WhenGettingApprenticeships
    {
        private ApprovalsController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetApprenticeshipsQueryResult _queryResult;
        private int _accountId;
        private string _status;
        private int _pageNumber;
        private int _pageItemCount;

        [SetUp]
        public void Setup()
        {
            _accountId = _fixture.Create<int>();
            _status = _fixture.Create<string>();
            _pageNumber = _fixture.Create<int>();
            _pageItemCount = _fixture.Create<int>();

            _mediator = new Mock<IMediator>();

            _queryResult = _fixture.Create<GetApprenticeshipsQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetApprenticeshipsQuery>(), CancellationToken.None))
                .ReturnsAsync(_queryResult);

            _controller = new ApprovalsController(_mediator.Object, Mock.Of<ILogger<ApprovalsController>>());
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Returned_Correctly()
        {
            var result = await _controller.GetApprenticeships(_accountId, _status, _pageNumber, _pageItemCount) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as GetApprenticeshipsResponse;
            Assert.That(response, Is.Not.Null);

            Assert.That(_queryResult.TotalApprenticeshipsFound, Is.EqualTo(response.TotalApprenticeshipsFound));
            CompareHelper.AreEqualIgnoringTypes(_queryResult.Apprenticeships.ToList(), response.Apprenticeships.ToList());
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Filtered_By_AccountId()
        {
            await _controller.GetApprenticeships(_accountId, _status, _pageNumber, _pageItemCount);
            _mediator.Verify(x => x.Send(It.Is<GetApprenticeshipsQuery>(q => q.AccountId == _accountId), CancellationToken.None));
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Filtered_By_Status()
        {
            await _controller.GetApprenticeships(_accountId, _status, _pageNumber, _pageItemCount);
            _mediator.Verify(x => x.Send(It.Is<GetApprenticeshipsQuery>(q => q.Status == _status), CancellationToken.None));
        }

        [Test]
        public async Task Then_Paging_Options_Are_Reflected()
        {
            await _controller.GetApprenticeships(_accountId, _status, _pageNumber, _pageItemCount);

            _mediator.Verify(x => x.Send(It.Is<GetApprenticeshipsQuery>(q => q.PageNumber == _pageNumber && q.PageItemCount == _pageItemCount), CancellationToken.None));
        }
    }
}