using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.CreateShortCourse;

public class CreateDraftShortCourseCommand : IRequest<CreateDraftShortCourseResult>
{
    public long Ukprn { get; set; }
    public ShortCourseRequest ShortCourseRequest { get; set; }
}
