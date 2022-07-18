using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AddressLookupControllerTests
    {
        private Mock<IMediator> _mediator;
        private AddressLookupController _sut;
        [SetUp]
        public void Before_Each_Test()
        {
            _mediator = new Mock<IMediator>();
            _sut = new AddressLookupController(_mediator.Object, Mock.Of<ILogger<AddressLookupController>>());
        }

        [Test]
        public async Task GetAddresses_InvalidPostcode_ReturnsBadRequest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<AddresssLookupQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((AddresssLookupQueryResult) null);

            var result = await _sut.GetAddresses("rubbish");

            var response = (BadRequestResult)result;

            response.Should().NotBeNull();
        }

        [Test]
        public async Task GetAddresses_ValidPostcode_ReturnsOkResponse()
        {
            var expectedResponse = new AddresssLookupQueryResult();
            _mediator.Setup(m => m.Send(It.IsAny<AddresssLookupQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            var result = await _sut.GetAddresses("CV1 1ET");

            var response = (OkObjectResult)result;

            response.Should().NotBeNull();
            response.Value.Should().Be(expectedResponse);
        }
    }
}
