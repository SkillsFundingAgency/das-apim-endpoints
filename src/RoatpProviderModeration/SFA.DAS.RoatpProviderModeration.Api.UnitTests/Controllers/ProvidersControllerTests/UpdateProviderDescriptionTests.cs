using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Api.Controllers;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProvidersControllerTests
{
    [TestFixture]
    public class UpdateProviderDescriptionTests
    {
        [Test, MoqAutoData]
        public async Task UpdateProviderDescription_Success_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController sut,
            int ukprn, UpdateProviderDescriptionCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateProviderDescriptionCommand>(c => c.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await sut.UpdateProviderDescription(ukprn, command);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task UpdateProviderDescription_Failed_ReturnsRespectiveStatusCode(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController sut,
            int ukprn, UpdateProviderDescriptionCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateProviderDescriptionCommand>(c => c.Ukprn == ukprn), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestContentException("some error", HttpStatusCode.BadRequest));

            var result = await sut.UpdateProviderDescription(ukprn, command);
            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }
    }
}
