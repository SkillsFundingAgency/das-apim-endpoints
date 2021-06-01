using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.ApprenticeshipDetails;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SaveApprenticeshipDetails
{
    public class SaveApprenticeshipDetailsCommand : IRequest
    {
        public ApprenticeshipDetailsRequest ApprenticeshipDetailsRequest { get; }

        public SaveApprenticeshipDetailsCommand(ApprenticeshipDetailsRequest request)
        {
            ApprenticeshipDetailsRequest = request;
        }
    }
}
