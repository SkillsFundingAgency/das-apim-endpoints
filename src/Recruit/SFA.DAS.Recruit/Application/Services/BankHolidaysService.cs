using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Services;

public interface IBankHolidaysService
{
    Task<BankHolidaysData> GetBankHolidayData();
}

public class BankHolidaysService : IBankHolidaysService
{
    private readonly HttpClient _httpClient;
    
    public BankHolidaysService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<BankHolidaysData> GetBankHolidayData()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.gov.uk/bank-holidays.json");
        
        var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
        
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        
        return JsonSerializer.Deserialize<BankHolidaysData>(json);
    }
}

public class Event
{
    public string Title { get; set; }
    public string Date { get; set; }
    public string Notes { get; set; }
    public bool Bunting { get; set; }

}

public class Data
{
    public string Division { get; set; }
    public List<Event> Events { get; set; }
}

public class BankHolidaysData
{
    [JsonPropertyName("england-and-wales")]
    public Data EnglandAndWales { get; set; }
    [JsonPropertyName("scotland")]
    public Data Scotland { get; set; }
    [JsonPropertyName("northern-ireland")]
    public Data NorthernIreland { get; set; }
}