﻿using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public CreatePledgeHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
        {
            var pledgeReference = await _levyTransferMatchingService.CreatePledge(request.Pledge);

            return new CreatePledgeResult()
            {
                PledgeReference = pledgeReference,
            };
        }
    }
}