using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCoursesControllerTests
{
    [TestFixture]
    public class ProviderCoursesControllerGetAllAvailableCoursesTests
    {
        [Test, MoqAutoData]
        public async Task GetAllAvailableCourses_ReturnsCourses(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProviderCoursesController sut,
            GetAvailableCoursesForProviderQueryResult response,
            int ukprn
            )
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetAvailableCoursesForProviderQuery>(q => q.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.GetAllAvailableCourses(ukprn);
            
            var okObjectResult = result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            var queryResult = okObjectResult.Value as GetAvailableCoursesForProviderQueryResult;
            queryResult.AvailableCourses.Count.Should().Be(response.AvailableCourses.Count);
        }
    }
}
