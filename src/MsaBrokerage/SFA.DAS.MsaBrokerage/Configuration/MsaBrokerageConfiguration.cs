using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.MsaBrokerage.Configuration;

[ExcludeFromCodeCoverage]
public class MsaBrokerageConfiguration
{
    public const int DefaultPageSizeValue = 50;
    public const int MaxPageSizeValue = 100;

    /// <summary>Page size used when the consumer does not supply one.</summary>
    public int DefaultPageSize { get; set; } = DefaultPageSizeValue;

    /// <summary>Largest page size a consumer may request.</summary>
    public int MaxPageSize { get; set; } = MaxPageSizeValue;

    /// <summary>
    /// Public base URL of the API as JUDE sees it through APIM, e.g. "https://msa-brokerage-outer-api.test/v1/".
    /// Used to build the absolute links on applicants. When empty, the links are relative.
    /// </summary>
    public string ApiBaseUrl { get; set; }

    /// <summary>Redis used to cache standard (course) lookups outside LOCAL/DEV. Same shared cache FAA uses.</summary>
    public string ApimEndpointsRedisConnectionString { get; set; }
}
