using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Jobs
{
    public class GetJobRunsQuery : IRequest<BaseMediatrResponse<GetJobRunsQueryResponse>> 
    {
        public string JobName { get; set; }
    }

}
