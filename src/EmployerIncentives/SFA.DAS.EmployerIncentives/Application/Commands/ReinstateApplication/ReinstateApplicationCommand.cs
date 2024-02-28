using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ReinstateApplication
{
    public class ReinstateApplicationCommand : IRequest<Unit>
    {
        public ReinstateApplicationRequest ReinstateApplicationRequest { get; }

        public ReinstateApplicationCommand(ReinstateApplicationRequest reinstateApplicationRequest)
        {
            ReinstateApplicationRequest = reinstateApplicationRequest;
        }
    }
}
