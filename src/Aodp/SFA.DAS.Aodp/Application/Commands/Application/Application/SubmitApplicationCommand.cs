using MediatR;
namespace SFA.DAS.Aodp.Application.Commands.Application.Application;
public class SubmitApplicationCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public Guid ApplicationId { get; set; }
    public string SubmittedBy { get; set; }
    public string SubmittedByEmail { get; set; }
}


