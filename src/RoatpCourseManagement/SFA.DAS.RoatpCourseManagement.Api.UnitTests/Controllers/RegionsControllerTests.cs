using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class RegionsControllerTests
    {
        [Test, MoqAutoData]
        public async Task GetAllRegions_InvokesMediator(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] RegionsController sut)
        {
            await sut.GetAllRegions();

            mediatorMock.Verify(m => m.Send(It.IsAny<GetAllRegionsQuery>(), It.IsAny<CancellationToken>()));
        }
    }
}
