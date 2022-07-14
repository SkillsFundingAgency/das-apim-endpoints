using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCourseEditControllerTests
{
    [TestFixture]
    public class UpdateProviderCourseApprovedByRegulatorTests
    {
        [Test, MoqAutoData]
        public async Task UpdateProviderCourseConfirmRegulatedStandard_Success_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseEditController sut,
            int ukprn, int larsCode, UpdateApprovedByRegulatorCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateApprovedByRegulatorCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await sut.UpdateProviderCourseApprovedByRegulator(ukprn, larsCode, command);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task UpdateProviderCourseConfirmRegulatedStandard_Failed_ReturnsRespectiveStatusCode(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseEditController sut,
            int ukprn, int larsCode, UpdateApprovedByRegulatorCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateApprovedByRegulatorCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestContentException("some error", HttpStatusCode.BadRequest));

            var result = await sut.UpdateProviderCourseApprovedByRegulator(ukprn, larsCode, command);
            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }
    }
}
