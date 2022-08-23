using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseCreateControllerTests
    {
        [Test, MoqAutoData]
        public async Task CreateProviderCourse_InvokesCreateProviderLocationCommand(
            [Frozen] Mock<IMediator> meditorMock,
            [Greedy] ProviderCourseCreateController sut,
            int ukprn,
            int larsCode,
            CreateProviderCourseCommand command)
        {
            await sut.CreateProviderCourse(ukprn, larsCode, command);

            meditorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
        }
    }
}
