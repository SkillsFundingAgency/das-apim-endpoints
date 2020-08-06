using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplication
{
    public class GetApplicationHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;
        private readonly ICommitmentsApiClient<CommitmentsConfiguration> _commitmentsV2Service;

        public GetApplicationHandler(IEmployerIncentivesService employerIncentivesService, ICommitmentsApiClient<CommitmentsConfiguration> commitmentsV2Service)
        {
            _employerIncentivesService = employerIncentivesService;
            _commitmentsV2Service = commitmentsV2Service;
        }
        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _employerIncentivesService.GetApplication(request.AccountId, request.ApplicationId);

            var applicationToReturn = new IncentiveApplication
            {
                AccountLegalEntityId = application.AccountLegalEntityId,
                Apprenticeships = await MapApprenticeships(application.Apprenticeships)
            };

            return new GetApplicationResult
            {
                Application = applicationToReturn
            };
        }

        private async Task<IEnumerable<IncentiveApplicationApprenticeship>> MapApprenticeships(IEnumerable<IncentiveApplicationApprenticeshipDto> applicationApprenticeships)
        {
            var commitmentApprenticeships = await GetCommitmentApprenticeships(applicationApprenticeships.Select(x => x.ApprenticeshipId));

            return applicationApprenticeships.Select(x => new IncentiveApplicationApprenticeship
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                TotalIncentiveAmount = x.TotalIncentiveAmount,
                CourseName = commitmentApprenticeships.SingleOrDefault(y => y.Id == x.ApprenticeshipId)?.CourseName
            });
        }

        private async Task<IEnumerable<GetApprenticeshipResponse>> GetCommitmentApprenticeships(IEnumerable<int> apprenticeshipIds)
        {
            var bag = new ConcurrentBag<GetApprenticeshipResponse>();
            var tasks = apprenticeshipIds.Select(async x =>
            {
                var apprenticeship = await _commitmentsV2Service.Get<GetApprenticeshipResponse>(new GetApprenticeshipRequest(x));
                bag.Add(apprenticeship);
            });
            await Task.WhenAll(tasks);
            return bag.ToArray();
        }
    }
}