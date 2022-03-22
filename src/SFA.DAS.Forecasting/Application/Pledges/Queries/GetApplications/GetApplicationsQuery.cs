using MediatR;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsQueryResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
