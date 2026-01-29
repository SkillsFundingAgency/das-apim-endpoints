using MediatR;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCoursesQuery(): IRequest<GetCoursesResult>
{
    public long Ukprn {  get; set; }
}
