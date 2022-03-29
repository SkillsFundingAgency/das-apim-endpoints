using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetStandardCourseDetailsQuery : IRequest<GetStandardsListItem>
    {
        public string CourseCode { get; set; }

        public GetStandardCourseDetailsQuery(string courseCode)
        {
            CourseCode = courseCode;
        }
    }
}
