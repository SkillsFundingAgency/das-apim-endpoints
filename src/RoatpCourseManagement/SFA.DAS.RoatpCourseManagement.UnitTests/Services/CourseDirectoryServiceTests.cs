using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services
{
    [TestFixture]
    public class CourseDirectoryServiceTests
    {
        [Test]
        public async Task GetAllProvidersData_OkResponse_ReturnsJsonContent()
        {
            var content = @"[{ 'id': 1, 'title': 'Cool post!'}, { 'id': 100, 'title': 'Some title'}]";
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var sut = new CourseDirectoryService(httpClient, Mock.Of<ILogger<CourseDirectoryService>>());
            
            var result = await sut.GetAllProvidersData();

            result.Should().Be(content);
        }

        [Test]
        public async Task GetAllProvidersData_UnsuccessfulResponse_ReturnsJsonContent()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var sut = new CourseDirectoryService(httpClient, Mock.Of<ILogger<CourseDirectoryService>>());

            Func<Task> action =() => sut.GetAllProvidersData();

            await action.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
