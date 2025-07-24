using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.PutUpsertEmploymentLocationsApiRequest;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record PutUpsertEmploymentLocationsApiRequest(Guid ApplicationId, Guid CandidateId, Guid Id, PutUpsertEmploymentLocationsApiRequestData Payload) : IPutApiRequest
    {
        public string PutUrl => $"api/candidates/{CandidateId}/applications/{ApplicationId}/employment-locations/{Id}";
        public object Data { get; set; } = Payload;

        public record PutUpsertEmploymentLocationsApiRequestData
        {
            public List<AddressDto> Addresses { get; set; } = [];
            public short EmployerLocationOption { get; set; }
            public string? EmploymentLocationInformation { get; set; }
        }
    }
}