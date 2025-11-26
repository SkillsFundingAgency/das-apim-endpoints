using System.Net;
using System.Net.Http;
using System.Text.Json;
using Moq.Protected;
using SFA.DAS.Recruit.Application.Services;

namespace SFA.DAS.Recruit.UnitTests.Application.Services;

public class BankHolidaysServiceTests
{
    [Test, AutoData]
    public async Task Then_The_BankHoliday_Data_Is_Returned(BankHolidaysData data)
    {
        var response = new HttpResponseMessage
        {
            Content = new StringContent(JsonSerializer.Serialize(data)),
            StatusCode = HttpStatusCode.OK
        };
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(c =>
                    c.Method.Equals(HttpMethod.Get)
                    && c.RequestUri!.AbsoluteUri.Equals("https://www.gov.uk/bank-holidays.json")
                    ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
        var client = new HttpClient(httpMessageHandler.Object);
        var clientFactory = new Mock<IHttpClientFactory>();
        clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        var bankHolidaysService = new BankHolidaysService(clientFactory.Object);
        
        var actual = await bankHolidaysService.GetBankHolidayData();
        
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(data);
    }
}