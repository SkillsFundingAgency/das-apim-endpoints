using System.Net;
using System.Text.Json;
using SFA.DAS.SharedOuterApi.Recruit.Services;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests.Services;

public class WhenFetchingBankHolidayData
{
    private static readonly JsonSerializerOptions JsonOptions = new(); 
    
    [Test, MoqAutoData]
    public async Task Then_The_Data_Is_Returned(Mock<IHttpClientFactory> httpClientFactory)
    {
        // arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(TestData.BankHolidaysJson)
        };
        var handler = new MockHttpMessageHandler([response]);
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(handler));
        
        var sut = new BankHolidaysService(httpClientFactory.Object);

        // act
        var result = await sut.GetBankHolidayDataAsync();

        // assert
        handler.Requests.Should().HaveCount(1);
        handler.Requests.First().RequestUri.AbsoluteUri.Should().Be("https://www.gov.uk/bank-holidays.json");
        result.Should().BeEquivalentTo(JsonSerializer.Deserialize<BankHolidaysData>(TestData.BankHolidaysJson, JsonOptions));
        result.EnglandAndWales.Events.Should().HaveCount(1);
        result.EnglandAndWales.Events[0].Title.Should().Be("Boxing Day");
        result.EnglandAndWales.Events[0].Date.Should().Be("2028-12-26");
        result.EnglandAndWales.Events[0].Notes.Should().BeEmpty();
    }
}

