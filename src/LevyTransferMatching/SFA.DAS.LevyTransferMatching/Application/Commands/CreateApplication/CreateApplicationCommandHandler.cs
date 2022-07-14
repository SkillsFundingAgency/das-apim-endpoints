using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IAccountsService _accountsService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILogger<CreateApplicationCommandHandler> _logger;

        public CreateApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService,
            IAccountsService accountsService,
            ILogger<CreateApplicationCommandHandler> logger,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _accountsService = accountsService;
            _logger = logger;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<CreateApplicationCommandResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating Application to Pledge {request.PledgeId} for Account {request.EmployerAccountId}");

            var accountTask = _levyTransferMatchingService.GetAccount(new GetAccountRequest(request.EmployerAccountId));
            var standardTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(request.StandardId));

            await Task.WhenAll(accountTask, standardTask);

            var account = accountTask.Result;
            var standard = standardTask.Result;

            if (account == null)
            {
                _logger.LogInformation($"Account {request.EmployerAccountId} does not exist - creating");
                await CreateAccount(request);
            }

            var data = new CreateApplicationRequestData
            {
                EmployerAccountId = request.EmployerAccountId,
                Details = request.Details,
                StandardId = request.StandardId,
                StandardTitle = standard.Title,
                StandardLevel = standard.Level,
                StandardDuration = standard.TypicalDuration,
                StandardMaxFunding = standard.MaxFundingOn(request.StartDate),
                StandardRoute = standard.Route,
                NumberOfApprentices = request.NumberOfApprentices,
                StartDate = request.StartDate,
                HasTrainingProvider = request.HasTrainingProvider,
                Sectors = request.Sectors,
                Locations = request.Locations,
                AdditionalLocation = request.AdditionalLocation,
                SpecificLocation = request.SpecificLocation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddresses = request.EmailAddresses,
                BusinessWebsite = request.BusinessWebsite,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            };

            var createApplicationRequest = new CreateApplicationRequest(request.PledgeId, data);

            var result = await _levyTransferMatchingService.CreateApplication(createApplicationRequest);

            return new CreateApplicationCommandResult
            {
                ApplicationId = result.ApplicationId
            };
        }

        private async Task CreateAccount(CreateApplicationCommand request)
        {
            var accountData = await _accountsService.GetAccount(request.EncodedAccountId);

            await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest(request.EmployerAccountId,
                accountData.DasAccountName));
        }
    }
}