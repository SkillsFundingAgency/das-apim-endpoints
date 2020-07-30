using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchHandler : IRequestHandler<GetEligibleApprenticeshipsSearchQuery, GetEligibleApprenticeshipsSearchResult>
    {
        private readonly ICommitmentsApiClient<CommitmentsConfiguration> _commitmentsV2Service;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetEligibleApprenticeshipsSearchHandler(
            ICommitmentsApiClient<CommitmentsConfiguration> commitmentsV2Service, 
            IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsV2Service = commitmentsV2Service;
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<GetEligibleApprenticeshipsSearchResult> Handle(GetEligibleApprenticeshipsSearchQuery request, CancellationToken cancellationToken)
        {
            var apprentices = await _commitmentsV2Service.Get<GetApprenticeshipListResponse>(new GetApprenticeshipsRequest(request.AccountId, request.AccountLegalEntityId));
            var result = new GetEligibleApprenticeshipsSearchResult
            {
                Apprentices = await _employerIncentivesService.GetEligibleApprenticeships(apprentices.Apprenticeships, cancellationToken)
            };

            return result;
        }
    }
}