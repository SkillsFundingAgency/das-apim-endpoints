using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand
{
    public class GetStartCourseDemandQuery : IRequest<GetStartCourseDemandQueryResult>
    {
        public int CourseId { get; set; }
    }
}