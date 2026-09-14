using System.Text.Json;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Recruit.Services;

public interface IBankHolidaysService
{
    Task<BankHolidaysData> GetBankHolidayDataAsync(CancellationToken cancellationToken = default);
}

public class BankHolidaysService(IHttpClientFactory httpClientFactory) : IBankHolidaysService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    public async Task<BankHolidaysData> GetBankHolidayDataAsync(CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.gov.uk/bank-holidays.json");
        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        
        return JsonSerializer.Deserialize<BankHolidaysData>(json)!;
    }
}

public class Event
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("date")]
    public required string Date { get; set; }
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
    [JsonPropertyName("bunting")]
    public bool Bunting { get; set; }
}

public class Data
{
    [JsonPropertyName("division")]
    public required string Division { get; set; }
    [JsonPropertyName("events")]
    public List<Event> Events { get; set; } = [];
}

public class BankHolidaysData
{
    [JsonPropertyName("england-and-wales")]
    public required Data EnglandAndWales { get; set; }
    [JsonPropertyName("scotland")]
    public required Data Scotland { get; set; }
    [JsonPropertyName("northern-ireland")]
    public required Data NorthernIreland { get; set; }
}