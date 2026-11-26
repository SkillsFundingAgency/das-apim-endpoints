using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record DeleteAccountApiRequest(Guid CandidateId) : IDeleteApiRequest
    {
        public string DeleteUrl => $"api/candidates/{CandidateId}";
    }
}