using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.Api.Controllers;
using SFA.DAS.Roatp.Application.Charities.Queries;
using SFA.DAS.Roatp.Domain.Models;

namespace SFA.DAS.Roatp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class CharitiesControllerTests
    {
        const int ValidRegistrationNumber = 101;
        const int InvalidRegistrationNumber = 999;
        [TestCase(0, 400)]
        [TestCase(-1, 400)]
        [TestCase(ValidRegistrationNumber, 200)]
        [TestCase(InvalidRegistrationNumber, 404)]
        public async Task GetCharity_ReturnsAppropriateResponse(int registrationNumber, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetCharityQuery>(q => q.RegistrationNumber == InvalidRegistrationNumber), It.IsAny<CancellationToken>())).ReturnsAsync(new GetCharityResult(null));
            mediatorMock.Setup(m => m.Send(It.Is<GetCharityQuery>(q => q.RegistrationNumber == ValidRegistrationNumber), It.IsAny<CancellationToken>())).ReturnsAsync(new GetCharityResult(new Charity()));

            var subject = new CharitiesController(mediatorMock.Object, Mock.Of<ILogger<CharitiesController>>());

            var response = await subject.GetCharity(registrationNumber);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.That(expectedStatusCode, Is.EqualTo(statusCodeResult.StatusCode.GetValueOrDefault()));
        }
    }
}
