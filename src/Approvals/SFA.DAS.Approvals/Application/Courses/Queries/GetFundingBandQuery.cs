using System;
using MediatR;

namespace SFA.DAS.Approvals.Application.Courses.Queries
{
    public class GetFundingBandQuery : IRequest<GetFundingBandResult>
    {
        public string CourseCode { get; set; }
        public DateTime? StartDate { get; set; }
    }
}