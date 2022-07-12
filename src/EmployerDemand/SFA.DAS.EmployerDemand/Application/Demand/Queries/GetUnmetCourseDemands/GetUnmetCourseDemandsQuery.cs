using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetCourseDemands
{
    public class GetUnmetCourseDemandsQuery : IRequest<GetUnmetCourseDemandsQueryResult>
    {
        public uint AgeOfDemandInDays { get; set; }
    }
}