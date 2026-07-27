using MediatR;

namespace SFA.DAS.LearnerData.Application.RemoveShortCourse;

public class RemoveShortCourseCommand : IRequest
{
    public long Ukprn { get; set; }
    public Guid LearnerKey { get; set; }
    public int AcademicYear { get; set; }
}
