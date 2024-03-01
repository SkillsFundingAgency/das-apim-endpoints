using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.UpdateProviderLocationDetails;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderLocationsEditControllerTests
{
    [TestFixture]
    public class UpdateProviderLocationDetailsTests
    {
        [Test, MoqAutoData]
        public async Task UpdateProviderLocationDetails_Success_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderLocationsEditController sut,
            int ukprn, Guid id, UpdateProviderLocationDetailsCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateProviderLocationDetailsCommand>(c => c.Ukprn == ukprn && c.Id == id), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.NoContent);

            var result = await sut.UpdateProviderLocationDetails(ukprn, id, command);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task UpdateProviderLocationDetails_Failed_ReturnsRespectiveStatusCode(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderLocationsEditController sut,
            int ukprn, Guid id, UpdateProviderLocationDetailsCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateProviderLocationDetailsCommand>(c => c.Ukprn == ukprn && c.Id == id), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.BadRequest);

            var result = await sut.UpdateProviderLocationDetails(ukprn, id, command);
            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }
    }
}
