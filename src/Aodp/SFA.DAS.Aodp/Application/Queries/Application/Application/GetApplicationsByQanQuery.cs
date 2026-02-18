using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationsByQanQuery : IRequest<BaseMediatrResponse<GetApplicationsByQanQueryResponse>>
{
    public GetApplicationsByQanQuery(string qan)
    {
        Qan = qan;
    }
    public string Qan { get; set; }
}
