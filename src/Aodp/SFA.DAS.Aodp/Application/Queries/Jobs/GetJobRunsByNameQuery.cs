using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Jobs
{
    public class GetJobRunsByNameQuery : IRequest<BaseMediatrResponse<GetJobRunsByNameQueryResponse>> 
    {
        public string JobName { get; set; }
    }

}
