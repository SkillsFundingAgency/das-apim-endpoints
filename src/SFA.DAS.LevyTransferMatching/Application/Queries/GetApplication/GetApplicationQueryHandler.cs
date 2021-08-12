using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _coursesApiClient = coursesApiClient;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));

            GetApplicationResult getApplicationResult = null;
            if (application != null)
            {
                var standard = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(application.StandardId));

                await Task.WhenAll(standard);

                // TODO: Get from location API
                string location = null;

                int estimatedDurationMonths = standard.Result.TypicalDuration;
                int level = standard.Result.Level;
                string typeOfJobRole = standard.Result.Title;

                getApplicationResult = new GetApplicationResult()
                {
                    AboutOpportunity = application.Details,
                    BusinessWebsite = application.BusinessWebsite,
                    EmailAddresses = application.EmailAddresses,
                    EstimatedDurationMonths = estimatedDurationMonths,
                    FirstName = application.FirstName,
                    HasTrainingProvider = application.HasTrainingProvider,
                    LastName = application.LastName,
                    Level = level,
                    Location = location,
                    NumberOfApprentices = application.NumberOfApprentices,
                    Sector = application.Sectors,
                    StartBy = application.StartDate,
                    TypeOfJobRole = typeOfJobRole,
                };
            }

            return getApplicationResult;
        }
    }
}