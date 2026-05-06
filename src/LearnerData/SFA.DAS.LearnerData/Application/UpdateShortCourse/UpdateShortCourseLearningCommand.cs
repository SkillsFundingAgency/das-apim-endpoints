using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.UpdateShortCourse;

public class UpdateShortCourseLearningCommand : IRequest
{
    public Guid LearningKey { get; set; }
    public long Ukprn { get; set; }
    public ShortCourseRequest Request { get; set; }
}
