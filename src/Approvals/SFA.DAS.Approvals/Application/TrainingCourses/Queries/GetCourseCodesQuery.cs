using MediatR;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCourseCodesQuery(): IRequest<GetCourseCodesResult>
{
    public long Ukprn {  get; set; }
}
