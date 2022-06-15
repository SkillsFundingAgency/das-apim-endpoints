using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand
{
    public class GetEmployerCourseProviderDemandQuery : IRequest<GetEmployerCourseProviderDemandQueryResult>
    {
        public string LocationName { get ; set ; }
        public int CourseId { get ; set ; }
        public int Ukprn { get ; set ; }
        public int? LocationRadius { get ; set ; }
    }
}