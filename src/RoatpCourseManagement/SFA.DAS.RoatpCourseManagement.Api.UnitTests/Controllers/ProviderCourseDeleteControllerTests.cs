using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseDeleteControllerTests
    {
        [Test, MoqAutoData]
        public async Task DeleteProviderCourse_InvokesCommand(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProviderCourseDeleteController sut,
            int ukprn, int larsCode, DeleteProviderCourseCommand command)
        {
            await sut.DeleteProviderCourse(ukprn, larsCode, command);

            mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
        }

        [Test, MoqAutoData]
        public async Task DeleteProviderCourse_ReturnsNoContent(
            [Greedy] ProviderCourseDeleteController sut,
            int ukprn, int larsCode, DeleteProviderCourseCommand command)
        {
            var response = await sut.DeleteProviderCourse(ukprn, larsCode, command);

            (response as NoContentResult).Should().NotBeNull();
        }
    }
}
