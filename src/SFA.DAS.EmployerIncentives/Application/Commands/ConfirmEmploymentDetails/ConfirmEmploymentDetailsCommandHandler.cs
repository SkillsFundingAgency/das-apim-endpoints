using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmEmploymentDetails
{
    public class ConfirmEmploymentDetailsCommandHandler : IRequestHandler<ConfirmEmploymentDetailsCommand>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public ConfirmEmploymentDetailsCommandHandler(ICommitmentsService commitmentsService, IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsService = commitmentsService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(ConfirmEmploymentDetailsCommand command, CancellationToken cancellationToken)
        {
            var apprenticeshipIds = command.ConfirmEmploymentDetailsRequest.EmploymentDetails.Select(x => x.ApprenticeId);
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(command.ConfirmEmploymentDetailsRequest.AccountId, apprenticeshipIds);

            var apprenticeshipDetails = new UpdateIncentiveApplicationRequestData
            {
                IncentiveApplicationId = command.ConfirmEmploymentDetailsRequest.ApplicationId,
                AccountId = command.ConfirmEmploymentDetailsRequest.AccountId,
                Apprenticeships = apprenticeships.Select(x => (IncentiveClaimApprenticeshipDto)x).ToArray()
            };
            foreach (var apprenticeship in apprenticeshipDetails.Apprenticeships)
            {
                var employmentDetails = command.ConfirmEmploymentDetailsRequest.EmploymentDetails.FirstOrDefault(x => x.ApprenticeId == apprenticeship.ApprenticeshipId);
                apprenticeship.EmploymentStartDate = employmentDetails.EmploymentStartDate;
            }

            await _employerIncentivesService.UpdateIncentiveApplication(apprenticeshipDetails);

            return Unit.Value;
        }
    }
}
