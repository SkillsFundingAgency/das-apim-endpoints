using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetCandidateByIdApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/{candidateId}";
}