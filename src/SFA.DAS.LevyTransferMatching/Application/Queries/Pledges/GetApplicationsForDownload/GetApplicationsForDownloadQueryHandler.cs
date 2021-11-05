using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsForDownload
{
    public class GetApplicationsForDownloadQueryHandler : IRequestHandler<GetApplicationsForDownloadQuery, GetApplicationsForDownloadQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;


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

                };
            }

            var distinctStandards = applications.Applications.Select(app => app.StandardId).Distinct();
            var standardTasks = new List<Task<GetStandardsListItem>>(distinctStandards.Count());

            standardTasks.AddRange(distinctStandards.Select(standardId => _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(standardId))));

            await Task.WhenAll(standardTasks);

            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);

            var applicationModels = new List<ApplicationForDownloadModel>();

            foreach (var application in applications.Applications)
            {
                var standard = standardTasks.Select(s => s.Result).Single(s => s.StandardUId == application.StandardId);
                var model = new ApplicationForDownloadModel
                {
                    Level = standard.Level,
                    TypeOfJobRole = standard.Title,
                    EstimatedDurationMonths = standard.TypicalDuration,
                    AboutOpportunity = application.Details,
                    AdditionalLocation = application.AdditionalLocation,
                    AllJobRoles = allJobRolesTask.Result,
                    AllLevels = allLevelsTask.Result,
                    AllSectors = allSectorsTask.Result,
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
                    SpecificLocation = application.SpecificLocation,
                    StartBy = application.StartDate,
                    Status = application.Status
                    // Locations = application.PledgeLocations?.Where(x => application.Locations.Select(y => y.PledgeLocationId).Contains(application.Id)).Select(x => application.Name),
                };

                applicationModels.Add(model);
            }

            return new GetApplicationsForDownloadQueryResult
            {
                Applications = applicationModels
            };
        }
    }
}
