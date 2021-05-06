using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentDetails;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmEmploymentDetails
{
    public class ConfirmEmploymentDetailsCommand : IRequest
    {
        public ConfirmEmploymentDetailsRequest ConfirmEmploymentDetailsRequest { get; }

        public ConfirmEmploymentDetailsCommand(ConfirmEmploymentDetailsRequest request)
        {
            ConfirmEmploymentDetailsRequest = request;
        }
    }
}
