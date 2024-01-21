using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PatchApplicationApiRequest : IPatchApiRequest<List<PatchApplicationRequest>>
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        public List<PatchApplicationRequest> Data { get; set; }

        public PatchApplicationApiRequest(
            Guid applicationId,
            Guid candidateId,
            List<PatchApplicationRequest> data)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            Data = data;
        }

        public string PatchUrl => $"api/Candidates/{_candidateId}/applications/{_applicationId}";
    }
}
