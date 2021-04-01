using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList
{
    public class GetAggregatedCourseDemandListQuery : IRequest<GetAggregatedCourseDemandListResult>
    {
        public int Ukprn { get; set; }
        public int? CourseId { get; set; }
        public string LocationName { get; set; }
    }
}