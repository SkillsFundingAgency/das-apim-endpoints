using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class UpdateApplicationAdditionalQuestionModel
{
    public SectionStatus? AdditionalQuestionOne { get; set; }
    public SectionStatus? AdditionalQuestionTwo { get; set; }
}
