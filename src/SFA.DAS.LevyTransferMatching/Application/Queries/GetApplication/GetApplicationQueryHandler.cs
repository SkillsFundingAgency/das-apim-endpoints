using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));

            GetApplicationResult getApplicationResult = null;
            if (application != null)
            {
                // TODO: Get from location API
                string location = null;

                // TODO: Get from standards API
                TimeSpan estimatedDuration = default(TimeSpan);
                string level = null;
                string typeOfJobRole = null;

                getApplicationResult = new GetApplicationResult()
                {
                    AboutOpportunity = application.Details,
                    BusinessWebsite = application.BusinessWebsite,
                    EmailAddresses = application.EmailAddresses,
                    EstimatedDuration = estimatedDuration,
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