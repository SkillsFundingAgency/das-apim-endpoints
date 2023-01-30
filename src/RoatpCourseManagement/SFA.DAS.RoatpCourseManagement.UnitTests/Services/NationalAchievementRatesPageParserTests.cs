using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services
{
    [TestFixture]
    public class NationalAchievementRatesPageParserTests
    {
        [Test]
        public async Task GetCurrentDownloadFilePath_FoundPath_ReturnsDownloadFilePath()
        {
            var yearTo = DateTime.Today.Year;
            var yearFrom = DateTime.Today.AddYears(-1).Year;
            var currentDownloadFile = $"{yearFrom} to {yearTo} apprenticeship NARTs overall CSVs";
            var nationalAchievementRatesDownloadPageUrl = "http://test.com";

            var content = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <title></title>
                            </head>
                            <body>
                               <div>
                                <h3>
	                            <a aria-describedby=""attachment-4178384-accessibility-help"" class=""govuk-link"" 
                                href=""https://test/202122_App_NARTs_Overall.zip"">" + currentDownloadFile + @"
                                </a>
	                            </h3>
                            </body>
                            </html>";
            
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
            var sut = new NationalAchievementRatesPageParser(Mock.Of<ILogger<NationalAchievementRatesPageParser>>(), httpClient);

            var result = await sut.GetCurrentDownloadFilePath(nationalAchievementRatesDownloadPageUrl);

            result.Should().NotBeNullOrEmpty();

            result.Should().Contain("https://test/202122_App_NARTs_Overall.zip");
        }

        [Test]
        public async Task GetCurrentDownloadFilePath_FoundPathButNoUrl_ReturnsEmptystring()
        {
            var nationalAchievementRatesDownloadPageUrl = "http://test.com";

            var content = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <title></title>
                            </head>
                            <body>
                               <div>
                                
                            </body>
                            </html>";

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
            var sut = new NationalAchievementRatesPageParser(Mock.Of<ILogger<NationalAchievementRatesPageParser>>(), httpClient);

            var result = await sut.GetCurrentDownloadFilePath(nationalAchievementRatesDownloadPageUrl);

            result.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task GetCurrentDownloadFilePath_NotFoundPath_ThrowsInvalidOperationException()
        {
            var nationalAchievementRatesDownloadPageUrl = "http://test.com";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var sut = new NationalAchievementRatesPageParser(Mock.Of<ILogger<NationalAchievementRatesPageParser>>(), httpClient);

            Func<Task> action = () => sut.GetCurrentDownloadFilePath(nationalAchievementRatesDownloadPageUrl);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
