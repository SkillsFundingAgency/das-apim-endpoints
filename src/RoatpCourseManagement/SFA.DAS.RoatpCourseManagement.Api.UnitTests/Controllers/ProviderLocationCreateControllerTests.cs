using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderLocationCreateControllerTests
    {
        [Test, MoqAutoData]
        public async Task CreateProviderLocation_InvokesCreateProviderLocationCommand(
            [Frozen] Mock<IMediator> meditorMock,
            [Greedy] ProviderLocationCreateController sut,
            int ukprn,
            CreateProviderLocationCommand command)
        {
            await sut.CreateProviderLocation(ukprn, command);
            meditorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
        }
    }
}
