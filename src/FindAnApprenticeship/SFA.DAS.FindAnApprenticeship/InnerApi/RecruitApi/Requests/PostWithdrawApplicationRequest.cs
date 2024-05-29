using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;

public class PostWithdrawApplicationRequest(Guid candidateId, long vacancyRef) : IPostApiRequest
{
    public string PostUrl => $"api/applications/{candidateId}/withdraw/{vacancyRef}";
    public object Data { get; set; }
}