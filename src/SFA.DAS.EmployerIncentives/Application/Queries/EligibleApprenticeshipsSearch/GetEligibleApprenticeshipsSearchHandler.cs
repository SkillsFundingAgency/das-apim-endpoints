using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchHandler : IRequestHandler<GetEligibleApprenticeshipsSearchQuery, GetEligibleApprenticeshipsSearchResult>
    {
        private readonly ICommitmentsV2Service _commitmentsV2Service;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetEligibleApprenticeshipsSearchHandler(ICommitmentsV2Service commitmentsV2Service, IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsV2Service = commitmentsV2Service;
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<GetEligibleApprenticeshipsSearchResult> Handle(GetEligibleApprenticeshipsSearchQuery request, CancellationToken cancellationToken)
        {
            var apprentices = await _commitmentsV2Service.Apprenticeships(request.AccountId, request.AccountLegalEntityId, cancellationToken);
            var result = new GetEligibleApprenticeshipsSearchResult { Apprentices = await _employerIncentivesService.GetEligibleApprenticeships(apprentices, cancellationToken)};

            return result;
        }
    }
}