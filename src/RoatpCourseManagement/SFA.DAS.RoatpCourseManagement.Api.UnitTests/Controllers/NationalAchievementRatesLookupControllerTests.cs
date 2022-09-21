using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class NationalAchievementRatesLookupControllerTests
    {
        private Mock<IMediator> _mediator;
        private NationalAchievementRatesLookupController _sut;
        [SetUp]
        public void Before_Each_Test()
        {
            _mediator = new Mock<IMediator>();
            _sut = new NationalAchievementRatesLookupController(_mediator.Object, Mock.Of<ILogger<NationalAchievementRatesLookupController>>());
        }

        [Test]
        public async Task GetNationalAcheivementRates_NullResponse_ReturnsBadRequest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<NationalAchievementRatesLookupQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((NationalAchievementRatesLookupQueryResult) null);

            var result = await _sut.GetNationalAcheivementRates();

            var response = (BadRequestResult)result;

            response.Should().NotBeNull();
        }

        [Test]
        public async Task GetNationalAcheivementRates_ValidResponse_ReturnsOkResponse()
        {
            var expectedResponse = new NationalAchievementRatesLookupQueryResult();
            _mediator.Setup(m => m.Send(It.IsAny<NationalAchievementRatesLookupQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            var result = await _sut.GetNationalAcheivementRates();

            var response = (OkObjectResult)result;

            response.Should().NotBeNull();
            response.Value.Should().Be(expectedResponse);
        }
    }
}
