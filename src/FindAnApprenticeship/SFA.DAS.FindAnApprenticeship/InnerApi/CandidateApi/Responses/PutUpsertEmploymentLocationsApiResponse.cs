using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public record PutUpsertEmploymentLocationsApiResponse
    {
        public Guid Id { get; set; }
        public List<AddressDto>? Addresses { get; set; } = [];
        public short EmployerLocationOption { get; set; }
        public string? EmploymentLocationInformation { get; set; }
        public Guid ApplicationId { get; set; }
    }
}