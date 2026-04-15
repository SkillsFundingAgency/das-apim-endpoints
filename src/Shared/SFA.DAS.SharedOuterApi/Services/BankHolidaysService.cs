using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services;

public interface IBankHolidaysService
{
    Task<BankHolidaysData> GetBankHolidayData();
}

public class BankHolidaysService(IHttpClientFactory httpClientFactory) : IBankHolidaysService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    private const string BankHolidaysUrl = "https://www.gov.uk/bank-holidays.json";

    public async Task<BankHolidaysData> GetBankHolidayData()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, BankHolidaysUrl);
        
        var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
        
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        
        return JsonSerializer.Deserialize<BankHolidaysData>(json);
    }
}

public record Event
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("date")]
    public string Date { get; set; }
    [JsonPropertyName("notes")]
    public string Notes { get; set; }
    [JsonPropertyName("bunting")]
    public bool Bunting { get; set; }

}

public record Data
{
    [JsonPropertyName("division")]
    public string Division { get; set; }
    [JsonPropertyName("events")]
    public List<Event> Events { get; set; }
}

public record BankHolidaysData
{
    [JsonPropertyName("england-and-wales")]
    public Data EnglandAndWales { get; set; }
    [JsonPropertyName("scotland")]
    public Data Scotland { get; set; }
    [JsonPropertyName("northern-ireland")]
    public Data NorthernIreland { get; set; }
}