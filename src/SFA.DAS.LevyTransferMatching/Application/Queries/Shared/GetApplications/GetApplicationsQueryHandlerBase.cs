using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications
{
    public abstract class GetApplicationsQueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, TResult> 
        where TQuery : GetApplicationsQueryBase, IRequest<TResult>
        where TResult : GetApplicationsQueryResultBase, new()
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        protected GetApplicationsQueryHandlerBase(ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _coursesApiClient = coursesApiClient;
        }

        public virtual async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                PledgeId = request?.PledgeId,
                AccountId = request?.AccountId
            });

            if (applicationsResponse.Applications == null)
            {
                return new TResult()
                {
                    Applications = new List<SharedOuterApi.Models.Application>()
                };
            }

            var distinctStandards = applicationsResponse.Applications.Select(app => app.StandardId).Distinct();
            var standardTasks = new List<Task<GetStandardsListItem>>(distinctStandards.Count());
            
            standardTasks.AddRange(distinctStandards.Select(standardId => _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(standardId))));

            await Task.WhenAll(standardTasks);

            foreach (var application in applicationsResponse.Applications)
            {
                var standard = standardTasks.Select(s => s.Result).Single(s => s.StandardUId == application.StandardId);
                application.Standard = new Standard()
                {
                    LarsCode = standard.LarsCode,
                    Level = standard.Level,
                    StandardUId = standard.StandardUId,
                    Title = standard.Title,
                    ApprenticeshipFunding = standard.ApprenticeshipFunding?.Select(funding =>
                        new ApprenticeshipFunding()
                        {
                            Duration = funding.Duration,
                            EffectiveFrom = funding.EffectiveFrom,
                            EffectiveTo = funding.EffectiveTo,
                            MaxEmployerLevyCap = funding.MaxEmployerLevyCap
                        })
                };
            }

            return new TResult
            {
                Applications = applicationsResponse.Applications
            };
        }
    }
}