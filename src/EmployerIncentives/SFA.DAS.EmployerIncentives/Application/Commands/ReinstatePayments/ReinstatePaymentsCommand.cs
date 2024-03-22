using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ReinstatePayments
{
    public class ReinstatePaymentsCommand : IRequest<Unit>
    {
        public ReinstatePaymentsRequest ReinstatePaymentsRequest { get; }

        public ReinstatePaymentsCommand(ReinstatePaymentsRequest reinstatePaymentsRequest)
        {
            ReinstatePaymentsRequest = reinstatePaymentsRequest;
        }
    }
}
