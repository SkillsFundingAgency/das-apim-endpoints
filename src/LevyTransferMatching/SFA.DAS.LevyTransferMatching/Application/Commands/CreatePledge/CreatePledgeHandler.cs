using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Models.Constants;

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

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating Pledge for account {request.AccountId}");

            var account = await _levyTransferMatchingService.GetAccount(new GetAccountRequest(request.AccountId));

            if (account == null)
            {
                _logger.LogInformation($"Account {request.AccountId} does not exist - creating");
                await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest(request.AccountId, request.DasAccountName));
            }

            var locationDataItems = new List<LocationDataItem>();
            foreach (var location in request.Locations)
            {
                var locationInformationResult = await _locationLookupService.GetLocationInformation(location, 0, 0);
                locationDataItems.Add(new LocationDataItem
                {
                    Name = locationInformationResult.Name,
                    GeoPoint = locationInformationResult.GeoPoint
                });
            }

            var apiRequest = new CreatePledgeRequest(request.AccountId, new CreatePledgeRequest.CreatePledgeRequestData
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                IsNamePublic = request.IsNamePublic,
                DasAccountName = request.DasAccountName,
                Sectors = request.Sectors,
                JobRoles = request.JobRoles,
                Levels = request.Levels,
                Locations = locationDataItems,
                AutomaticApprovalOption = string.IsNullOrWhiteSpace(request.AutomaticApprovalOption)
                    ? AutomaticApprovalOption.Default
                    : request.AutomaticApprovalOption,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            var result = await _levyTransferMatchingService.CreatePledge(apiRequest);

            return new CreatePledgeResult
            {
                PledgeId = result.Id,
            };
        }
    }
}