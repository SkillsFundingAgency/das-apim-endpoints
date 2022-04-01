using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetStandardQuery : IRequest<GetStandardsListItem>
    {
        public string CourseCode { get; set; }

        public GetStandardQuery(string courseCode)
        {
            CourseCode = courseCode;
        }
    }
}
