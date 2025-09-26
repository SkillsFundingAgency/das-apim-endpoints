using System;
using SFA.DAS.Apprenticeships.Types;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

public class GetLearnerStatusResponse
{
    public LearnerStatus? LearnerStatus { get; set; }
    public DateTime? WithdrawalChangedDate { get; set; }
    public string WithdrawalReason { get; set; }
    public DateTime? LastCensusDateOfLearning { get; set; }
    public DateTime? LastDayOfLearning { get; set; }
}