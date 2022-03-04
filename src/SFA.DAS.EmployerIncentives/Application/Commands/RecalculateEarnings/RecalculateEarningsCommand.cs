using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RecalculateEarnings
{
    public class RecalculateEarningsCommand : IRequest
    {
        public RecalculateEarningsRequest RecalculateEarningsRequest { get; }
        public RecalculateEarningsCommand(RecalculateEarningsRequest request)
        {
            RecalculateEarningsRequest = request;
        }
    }
}
