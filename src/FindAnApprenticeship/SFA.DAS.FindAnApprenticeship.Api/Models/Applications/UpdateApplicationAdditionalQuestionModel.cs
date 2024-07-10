using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class UpdateApplicationAdditionalQuestionModel
{
    public SectionStatus? AdditionalQuestionOne { get; set; }
    public SectionStatus? AdditionalQuestionTwo { get; set; }
}
