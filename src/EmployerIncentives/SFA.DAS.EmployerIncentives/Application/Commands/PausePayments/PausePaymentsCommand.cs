using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.PausePayments
{
    public class PausePaymentsCommand : IRequest
    {   
        public PausePaymentsRequest PausePaymentsRequest { get; }

        public PausePaymentsCommand(PausePaymentsRequest pausePaymentsRequest)
        {
            PausePaymentsRequest = pausePaymentsRequest;
        }
    }
}
