using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            foreach(var location in command.Locations)
            {
                var locationInformationResult = await _locationLookupService.GetLocationInformation(location.Name, location.GeoPoint[0], location.GeoPoint[1]);
                location.GeoPoint = locationInformationResult.GeoPoint;
            }

            var pledgeId = await _levyTransferMatchingService.CreatePledge(command);

            return new CreatePledgeResult
            {
                PledgeId = pledgeId
            };
        }
    }
}