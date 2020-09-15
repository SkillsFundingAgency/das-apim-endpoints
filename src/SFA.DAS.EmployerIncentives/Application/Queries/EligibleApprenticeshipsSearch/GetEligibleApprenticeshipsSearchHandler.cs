using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchHandler : IRequestHandler<GetEligibleApprenticeshipsSearchQuery, GetEligibleApprenticeshipsSearchResult>
    {
        private readonly ICommitmentsService _commitmentsV2Service;
        private readonly IEmployerIncentivesService _employerIncentivesService;
        private readonly ILogger<GetEligibleApprenticeshipsSearchHandler> _logger;

        public GetEligibleApprenticeshipsSearchHandler(
            ICommitmentsService commitmentsV2Service, 
            IEmployerIncentivesService employerIncentivesService,
            ILogger<GetEligibleApprenticeshipsSearchHandler> logger)
        {
            _commitmentsV2Service = commitmentsV2Service;
            _employerIncentivesService = employerIncentivesService;
            _logger = logger;
        }

        public async Task<GetEligibleApprenticeshipsSearchResult> Handle(GetEligibleApprenticeshipsSearchQuery request, CancellationToken cancellationToken)
        {
            var incentiveDetails = await _employerIncentivesService.GetIncentiveDetails();
            var apprentices = await _commitmentsV2Service.Apprenticeships(request.AccountId, request.AccountLegalEntityId, incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate);
            _logger.LogInformation($"Retrieved {apprentices.Count()} apprenticeships for account {request.AccountId} legal entity {request.AccountLegalEntityId}");

            var result = new GetEligibleApprenticeshipsSearchResult
            {
                Apprentices = await _employerIncentivesService.GetEligibleApprenticeships(request.AccountId, request.AccountLegalEntityId, apprentices)
            };
            _logger.LogInformation($"Resolved {result.Apprentices.Count()} eligible apprenticeships for account {request.AccountId} legal entity {request.AccountLegalEntityId}");

            return result;
        }
    }
}