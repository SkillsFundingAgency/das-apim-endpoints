using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsForDownload
{
    public class GetApplicationsForDownloadQueryHandler : IRequestHandler<GetApplicationsForDownloadQuery, GetApplicationsForDownloadQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;
        private List<ReferenceDataItem> _allJobRoles;
        private List<ReferenceDataItem> _allLevels;
        private List<ReferenceDataItem> _allSectors;


        public GetApplicationsForDownloadQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _coursesApiClient = coursesApiClient;
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetApplicationsForDownloadQueryResult> Handle(GetApplicationsForDownloadQuery request, CancellationToken cancellationToken)
        {
            var applications = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                AccountId = request.AccountId,
                PledgeId = request.PledgeId
            });

            if (!applications.Applications.Any())
            {
                return new GetApplicationsForDownloadQueryResult
                {
                    Applications = new List<ApplicationForDownloadModel>()
                };
            }

            var standardTasks = RetrieveStandardTasks(applications);

            await RetrieveFromReferenceDataService();

            var applicationModels = new List<ApplicationForDownloadModel>();

            foreach (var application in applications.Applications)
            {
                var standardListItem = standardTasks.First(s => s?.StandardUId == application.StandardId);
                var standard = BuildStandardFromListItem(standardListItem);
                var roles = _allJobRoles.Where(x => application.PledgeJobRoles.Contains(x.Id));

                BuildApplicationModelAndInsertIntoList(application, standard, standardListItem, roles, applicationModels);
            }

            return new GetApplicationsForDownloadQueryResult
            {
                Applications = applicationModels
            };
        }

        private static void BuildApplicationModelAndInsertIntoList(SharedOuterApi.Models.Application application, Standard standard,
            GetStandardsListItem standardListItem, IEnumerable<ReferenceDataItem> roles, List<ApplicationForDownloadModel> applicationModels)
        {
            var model = new ApplicationForDownloadModel
            {
                AboutOpportunity = application.Details,
                ApplicationId = application.Id,
                BusinessWebsite = application.BusinessWebsite,
                DateApplied = application.CreatedOn,
                EmailAddresses = application.EmailAddresses,
                EmployerAccountName = application.DasAccountName,
                FirstName = application.FirstName,
                HasTrainingProvider = application.HasTrainingProvider,
                LastName = application.LastName,
                NumberOfApprentices = application.NumberOfApprentices,
                PledgeId = application.PledgeId,
                Sectors = application.Sectors,
                StartBy = application.StartDate,
                Status = application.Status,
                Standard = standard,
                EstimatedDurationMonths = standardListItem.TypicalDuration,
                MaxFunding = standardListItem.MaxFunding,
                IsLocationMatch = !application.PledgeLocations.Any() || application.Locations.Any(),
                IsSectorMatch = !application.PledgeSectors.Any() ||
                                application.Sectors.Any(x => application.PledgeSectors.Contains(x)),
                IsJobRoleMatch = !application.PledgeJobRoles.Any() || roles.Any(r => r.Description == standard.Route),
                IsLevelMatch = !application.PledgeLevels.Any() || application.PledgeLevels
                    .Select(x => char.GetNumericValue(x.Last())).Contains(standard.Level),
                PledgeRemainingAmount = application.PledgeRemainingAmount,
                Amount = application.Amount,
                JobRole = standardListItem.Title
            };

            applicationModels.Add(model);
        }

        private static Standard BuildStandardFromListItem(GetStandardsListItem standardListItem)
        {
            var standard = new Standard()
            {
                LarsCode = standardListItem.LarsCode,
                Level = standardListItem.Level,
                StandardUId = standardListItem.StandardUId,
                Title = standardListItem.Title,
                ApprenticeshipFunding = standardListItem.ApprenticeshipFunding?.Select(funding =>
                    new ApprenticeshipFunding()
                    {
                        Duration = funding.Duration,
                        EffectiveFrom = funding.EffectiveFrom,
                        EffectiveTo = funding.EffectiveTo,
                        MaxEmployerLevyCap = funding.MaxEmployerLevyCap,
                    })
            };
            return standard;
        }

        private async Task RetrieveFromReferenceDataService()
        {
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);
            
            _allJobRoles = allJobRolesTask.Result;
            _allLevels = allLevelsTask.Result;
            _allSectors = allSectorsTask.Result;
        }

        private List<GetStandardsListItem> RetrieveStandardTasks(GetApplicationsResponse applications)
        {
            var distinctStandards = applications.Applications.Select(app => app.StandardId).Distinct();
            var standardTasks = new List<GetStandardsListItem>(distinctStandards.Count());
            var returnedStandards = distinctStandards.Select(async standardId =>
                await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(standardId)));

            standardTasks.AddRange(returnedStandards.Select(task => task.Result).Where(task => task != null));

            return standardTasks;
        }
    }
}
