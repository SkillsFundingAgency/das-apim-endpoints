using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseLocationDeleteControllerTests
    {
        [Test, MoqAutoData]
        public async Task BulkDeleteProviderCourseLocations_InvokesCommand(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProviderCourseLocationDeleteController sut,
            int ukprn, int larsCode, BulkDeleteProviderCourseLocationsCommand command)
        {
            await sut.BulkDeleteProviderCourseLocations(ukprn, larsCode, command);

            mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
        }

        [Test, MoqAutoData]
        public async Task BulkDeleteProviderCourseLocations_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProviderCourseLocationDeleteController sut,
            int ukprn, int larsCode, BulkDeleteProviderCourseLocationsCommand command)
        {
            var response = await sut.BulkDeleteProviderCourseLocations(ukprn, larsCode, command);

            (response as NoContentResult).Should().NotBeNull();
        }
    }
}
