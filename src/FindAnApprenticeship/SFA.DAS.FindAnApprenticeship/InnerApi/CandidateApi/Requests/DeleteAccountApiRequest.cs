using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record DeleteAccountApiRequest(Guid CandidateId) : IDeleteApiRequest
    {
        public string DeleteUrl => $"api/candidates/{CandidateId}";
    }
}