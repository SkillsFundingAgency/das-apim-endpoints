using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.WithdrawApplication
{
    public class WithdrawCommand : IRequest
    {   
        public WithdrawRequest WithdrawRequest { get; }

        public WithdrawCommand(WithdrawRequest withdrawRequest)
        {
            WithdrawRequest = withdrawRequest;
        }
    }
}
