using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetAboutYouItemApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/{candidateId}/about-you";
}
