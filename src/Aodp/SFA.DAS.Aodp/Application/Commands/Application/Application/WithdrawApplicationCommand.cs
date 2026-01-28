using MediatR;
namespace SFA.DAS.Aodp.Application.Commands.Application.Application;
public class WithdrawApplicationCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public Guid ApplicationId { get; set; }
    public required string WithdrawnBy { get; set; }
    public required string WithdrawnByEmail { get; set; }
}