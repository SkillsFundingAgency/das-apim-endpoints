using System.Text.Json.Serialization;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

public class GetClosedVacancyApiResponse
{
    public string? Title { get; set; }
    public string? EmployerName { get; set; }
    public SharedOuterApi.Models.Address? Address { get; set; }
    [JsonPropertyName("employerLocationOption"), JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
    public AvailableWhere? EmployerLocationOption { get; set; }
    public List<SharedOuterApi.Models.Address>? EmployerLocations { get; set; } = [];
    public List<SharedOuterApi.Models.Address>? OtherAddresses
    {
        get
        {
            if (EmployerLocationOption is not AvailableWhere.MultipleLocations) return null;
            var otherAddresses = EmployerLocations?
                .DistinctBy(x => x.ToSingleLineAddress())
                .ToList();
            return otherAddresses;
        }
    }
}