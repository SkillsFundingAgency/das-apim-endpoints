using MediatR;
using SFA.DAS.LearnerData.Application.CreateShortCourse;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.CreateShortCourseLearning;

#pragma warning disable CS8618
public class CreateDraftShortCourseCommand : IRequest<CreateDraftShortCourseResult>
{
    public long Ukprn { get; set; }
    public ShortCourseRequest ShortCourseRequest { get; set; }
}
#pragma warning restore CS8618