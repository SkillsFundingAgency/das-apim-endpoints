using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchHandler : IRequestHandler<GetEligibleApprenticeshipsSearchQuery, GetEligibleApprenticeshipsSearchResult>
    {
        private readonly ICommitmentsService _commitmentsV2Service;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetEligibleApprenticeshipsSearchHandler(
            ICommitmentsService commitmentsV2Service, 
            IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsV2Service = commitmentsV2Service;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<GetEligibleApprenticeshipsSearchResult> Handle(GetEligibleApprenticeshipsSearchQuery request, CancellationToken cancellationToken)
        {
            var incentiveDetails = await _employerIncentivesService.GetIncentiveDetails();

            var apprenticesResponse = await _commitmentsV2Service.Apprenticeships(
                    request.AccountId, request.AccountLegalEntityId,
                    incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate,
                    request.PageNumber, request.PageSize);

            var validApprenticeships = apprenticesResponse.Apprenticeships.Where(x => x.ApprenticeshipStatus != ApprenticeshipStatus.Stopped).ToList();

            if (!validApprenticeships.Any())
            {
                return new GetEligibleApprenticeshipsSearchResult
                {
                    Apprentices = new ApprenticeshipItem[0],
                    PageNumber = request.PageNumber,
                    TotalApprenticeships = apprenticesResponse.TotalApprenticeshipsFound
                };
            }

            var filteredApprenticeships = await _employerIncentivesService.GetEligibleApprenticeships(validApprenticeships);

            var result = new GetEligibleApprenticeshipsSearchResult
            {
                Apprentices = filteredApprenticeships,
                PageNumber = request.PageNumber,
                TotalApprenticeships = apprenticesResponse.TotalApprenticeshipsFound
            };

            return result;
        }
    }
}