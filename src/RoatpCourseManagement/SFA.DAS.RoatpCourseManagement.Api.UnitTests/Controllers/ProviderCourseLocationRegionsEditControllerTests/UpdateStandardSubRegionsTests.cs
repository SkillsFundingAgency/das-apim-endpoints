using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCourseLocationRegionsEditControllerTests
{
    [TestFixture]
    public class UpdateStandardSubRegionsTests
    {
        [Test, MoqAutoData]
        public async Task UpdateSubregions_Success_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseLocationRegionsEditController sut,
            int ukprn, int larsCode, UpdateStandardSubRegionsCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateStandardSubRegionsCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.NoContent);

            var result = await sut.UpdateStandardSubRegions(ukprn, larsCode, command);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task UpdateSubregions_Failed_ReturnsRespectiveStatusCode(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseLocationRegionsEditController sut,
            int ukprn, int larsCode, UpdateStandardSubRegionsCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateStandardSubRegionsCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.BadRequest);

            var result = await sut.UpdateStandardSubRegions(ukprn, larsCode, command);
            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }
    }
}
