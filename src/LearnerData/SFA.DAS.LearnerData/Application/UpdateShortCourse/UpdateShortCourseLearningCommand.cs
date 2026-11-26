using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.UpdateShortCourse;

public class UpdateShortCourseLearningCommand : IRequest
{
    public Guid LearnerKey { get; set; }
    public long Ukprn { get; set; }
    public int AcademicYear { get; set; }
    public ShortCourseRequest Request { get; set; }
}
