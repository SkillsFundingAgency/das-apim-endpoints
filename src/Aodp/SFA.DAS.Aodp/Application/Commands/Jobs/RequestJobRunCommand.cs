using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Jobs;

public class RequestJobRunCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public string JobName { get; set; }
    public string UserName { get; set; }
}
