using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand
{
    public class GetRegisterDemandQuery : IRequest<GetRegisterDemandResult>
    {
        public int CourseId { get; set; }
        public string LocationName { get ; set ; }
    }
}