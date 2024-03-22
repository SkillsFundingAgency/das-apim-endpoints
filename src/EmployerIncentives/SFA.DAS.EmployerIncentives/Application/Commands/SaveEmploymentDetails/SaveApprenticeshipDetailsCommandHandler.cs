using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SaveApprenticeshipDetails
{
    public class SaveApprenticeshipDetailsCommandHandler : IRequestHandler<SaveApprenticeshipDetailsCommand, Unit>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IApplicationService _applicationService;

        public SaveApprenticeshipDetailsCommandHandler(ICommitmentsService commitmentsService, IApplicationService applicationService)
        {
            _commitmentsService = commitmentsService;
            _applicationService = applicationService;
        }

        public async Task<Unit> Handle(SaveApprenticeshipDetailsCommand request, CancellationToken cancellationToken)
        {
            var apprenticeshipIds = request.ApprenticeshipDetailsRequest.ApprenticeshipDetails.Select(x => x.ApprenticeId);
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(request.ApprenticeshipDetailsRequest.AccountId, apprenticeshipIds);

            var apprenticeshipDetails = new UpdateIncentiveApplicationRequestData
            {
                IncentiveApplicationId = request.ApprenticeshipDetailsRequest.ApplicationId,
                AccountId = request.ApprenticeshipDetailsRequest.AccountId,
                Apprenticeships = apprenticeships.Select(x => (IncentiveClaimApprenticeshipDto)x).ToArray()
            };
            foreach (var apprenticeship in apprenticeshipDetails.Apprenticeships)
            {
                var employmentDetails = request.ApprenticeshipDetailsRequest.ApprenticeshipDetails.FirstOrDefault(x => x.ApprenticeId == apprenticeship.ApprenticeshipId);
                apprenticeship.EmploymentStartDate = employmentDetails.EmploymentStartDate;
            }

            await _applicationService.Update(apprenticeshipDetails);

            return Unit.Value;
        }
    }
}
