using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RevertPayments
{
    public class RevertPaymentsCommand : IRequest<Unit>
    {
        public RevertPaymentsRequest RevertPaymentsRequest { get; }

        public RevertPaymentsCommand(RevertPaymentsRequest revertPaymentsRequest)
        {
            RevertPaymentsRequest = revertPaymentsRequest;
        }
    }
}