using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared
{
    public record LocationDto
    {
        [JsonProperty("addresses")]
        public List<AddressDto> Addresses { get; set; } = null!;
        [JsonProperty("employerLocationOption")]
        public AvailableWhere? EmployerLocationOption { get; set; }
        [JsonProperty("employmentLocationInformation")]
        public string EmploymentLocationInformation { get; set; } = null;
    }
}
