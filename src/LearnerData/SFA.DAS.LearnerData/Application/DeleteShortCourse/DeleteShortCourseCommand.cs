using MediatR;

namespace SFA.DAS.LearnerData.Application.DeleteShortCourse;

public class DeleteShortCourseCommand : IRequest
{
    public long Ukprn { get; set; }
    public Guid LearningKey { get; set; }
}
