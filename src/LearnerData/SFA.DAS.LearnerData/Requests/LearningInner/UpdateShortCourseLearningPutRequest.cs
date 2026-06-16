using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Enums;

namespace SFA.DAS.LearnerData.Requests.LearningInner;


/// <summary>
/// This request will be sent to the Learning Inner Api
/// </summary>
public class UpdateShortCourseLearningPutRequest : IPutApiRequest<UpdateShortCourseLearningRequestBody>
{
    public string PutUrl { get; }
    public UpdateShortCourseLearningRequestBody Data { get; set; }

    public UpdateShortCourseLearningPutRequest(Guid learnerKey, UpdateShortCourseLearningRequestBody data)
    {
        PutUrl = $"shortCourses/{learnerKey}";
        Data = data;
    }
}
#pragma warning disable CS8618
public class UpdateShortCourseLearningRequestBody
{
    public long Ukprn { get; set; }
    public ShortCourseLearnerUpdateDetails LearnerUpdateDetails { get; set; }
    public List<ShortCourseOnProgrammeUpdateDetails> OnProgramme { get; set; } = new();
}

public class ShortCourseLearnerUpdateDetails
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string LearnerRef { get; set; }
}

public class ShortCourseOnProgrammeUpdateDetails
{
    public long Ukprn { get; set; }
    public string CourseCode { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public List<Milestone> Milestones { get; set; } = new();
}

#pragma warning restore CS8618
