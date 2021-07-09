using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILocationLookupService _locationLookupService;

        public CreatePledgeHandler(ILevyTransferMatchingService levyTransferMatchingService, ILocationLookupService locationLookupService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _locationLookupService = locationLookupService;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
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

            var pledgeId = await _levyTransferMatchingService.CreatePledge(pledge);

            return new CreatePledgeResult
            {
                PledgeId = pledgeId
            };
        }
    }
}