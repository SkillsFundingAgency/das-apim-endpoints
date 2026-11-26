using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Domain;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared
{
    public record LocationDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("addresses")] 
        public List<AddressDto> Addresses { get; set; } = [];
        [JsonProperty("employerLocationOption")]
        public AvailableWhere? EmployerLocationOption { get; set; }
        [JsonProperty("employmentLocationInformation")]
        public string? EmploymentLocationInformation { get; set; } = null;
    }
}
