using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchHandler : IRequestHandler<GetEligibleApprenticeshipsSearchQuery, GetEligibleApprenticeshipsSearchResult>
    {
        private readonly ICommitmentsService _commitmentsV2Service;
        private readonly IEmployerIncentivesService _employerIncentivesService;
        private readonly IApprenticeshipService _apprenticeshipService;

        public GetEligibleApprenticeshipsSearchHandler(
            ICommitmentsService commitmentsV2Service, 
            IEmployerIncentivesService employerIncentivesService,
            IApprenticeshipService apprenticeshipService)
        {
            _commitmentsV2Service = commitmentsV2Service;
            _employerIncentivesService = employerIncentivesService;
            _apprenticeshipService = apprenticeshipService;
        }

        public async Task<GetEligibleApprenticeshipsSearchResult> Handle(GetEligibleApprenticeshipsSearchQuery request, CancellationToken cancellationToken)
        {
            var incentiveDetails = await _employerIncentivesService.GetIncentiveDetails();
            var apprentices = await _commitmentsV2Service.Apprenticeships(request.AccountId, request.AccountLegalEntityId, incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate);
            var result = new GetEligibleApprenticeshipsSearchResult
            {
                Apprentices = await _apprenticeshipService.GetEligibleApprenticeships(apprentices)
            };

            return result;
        }
    }
}