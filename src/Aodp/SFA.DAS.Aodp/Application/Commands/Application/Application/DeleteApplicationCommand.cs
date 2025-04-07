using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class DeleteApplicationCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public Guid ApplicationId { get; set; }

    public DeleteApplicationCommand(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
