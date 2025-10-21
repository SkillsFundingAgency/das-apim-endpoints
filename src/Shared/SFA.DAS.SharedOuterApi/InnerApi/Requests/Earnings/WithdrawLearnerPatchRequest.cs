using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;

public class WithdrawLearnerPatchRequest : IPatchApiRequest<WithdrawLearnerRequest>
{
    public WithdrawLearnerPatchRequest(Guid learningKey, DateTime withdrawalDate)
    {
        LearningKey = learningKey;
        Data = new WithdrawLearnerRequest
        {
            WithdrawalDate = withdrawalDate
        };
    }

    public Guid LearningKey { get; set; }
    public string PatchUrl => $"apprenticeship/{LearningKey}/withdraw";
    public WithdrawLearnerRequest Data { get; set; }
}

public class WithdrawLearnerRequest
{
    public DateTime WithdrawalDate { get; set; }
}