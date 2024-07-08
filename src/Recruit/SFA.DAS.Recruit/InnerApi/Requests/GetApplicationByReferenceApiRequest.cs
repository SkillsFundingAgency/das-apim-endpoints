using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetApplicationByReferenceApiRequest(Guid candidateId, long vacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/{candidateId}/applications/GetByReference/{vacancyReference}";
}