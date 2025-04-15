using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Jobs
{
    public class GetJobByNameQuery : IRequest<BaseMediatrResponse<GetJobByNameQueryResponse>> 
    {
        public string JobName { get; set; }
    }

}
