using System;
using SFA.DAS.SharedOuterApi.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PatchApplicationApiRequest : IPatchApiRequest<JsonPatchDocument<Models.Application>>
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        public JsonPatchDocument<Models.Application> Data { get; set; }

        public PatchApplicationApiRequest(
            Guid applicationId,
            Guid candidateId,
            JsonPatchDocument<Models.Application> data)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            Data = data;
        }

        public string PatchUrl => $"api/Candidates/{_candidateId}/applications/{_applicationId}";
    }
}
