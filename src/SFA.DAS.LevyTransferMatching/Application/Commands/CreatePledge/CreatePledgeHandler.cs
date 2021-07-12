using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILocationLookupService _locationLookupService;
        private readonly ILogger<CreatePledgeHandler> _logger;

        public CreatePledgeHandler(ILevyTransferMatchingService levyTransferMatchingService, ILocationLookupService locationLookupService, ILogger<CreatePledgeHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _locationLookupService = locationLookupService;
            _logger = logger;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating Pledge for account {command.AccountId}");

            var account = await _levyTransferMatchingService.GetAccount(new GetAccountRequest(command.AccountId));

            if (account == null)
            {
                _logger.LogInformation($"Account {command.AccountId} does not exist - creating");
                await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest(command.AccountId, command.DasAccountName));
            }

            var locationDataItems = new List<LocationDataItem>();
            foreach (var location in command.Locations)
            {
                var locationInformationResult = await _locationLookupService.GetLocationInformation(location, 0, 0);
                locationDataItems.Add(new LocationDataItem
                {
                    Name = locationInformationResult.Name,
                    GeoPoint = locationInformationResult.GeoPoint
                });
            }

            var pledge = new Pledge
            {
                AccountId = command.AccountId,
                Amount = command.Amount,
                IsNamePublic = command.IsNamePublic,
                DasAccountName = command.DasAccountName,
                Sectors = command.Sectors,
                JobRoles = command.JobRoles,
                Levels = command.Levels,
                Locations = locationDataItems
            };

            var pledgeReference = await _levyTransferMatchingService.CreatePledge(pledge);

            return new CreatePledgeResult
            {
                PledgeId = pledgeReference.Id.Value,
            };
        }
    }
}