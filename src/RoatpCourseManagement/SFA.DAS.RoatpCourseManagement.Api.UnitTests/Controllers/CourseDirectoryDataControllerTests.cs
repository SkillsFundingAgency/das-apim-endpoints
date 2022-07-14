using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class CourseDirectoryDataControllerTests
    {
        [Test]
        [MoqAutoData]
        public async Task GetProvidersData_ReturnsJsonContent(
            [Frozen] Mock<ICourseDirectoryService> serviceMock,
            [Greedy] CourseDirectoryDataController sut)
        {
            var content = @"[{ 'id': 1, 'title': 'Cool post!'}, { 'id': 100, 'title': 'Some title'}]";
            var expectedResult = new JsonResult(content);
            serviceMock.Setup(s => s.GetAllProvidersData()).ReturnsAsync(content);

            var result = await sut.GetProvidersData();

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
