using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication
{
    public class DebitApplicationCommandHandler : IRequestHandler<DebitApplicationCommand, DebitApplicationCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public DebitApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<DebitApplicationCommandResult> Handle(DebitApplicationCommand request, CancellationToken cancellationToken)
        {
            var data = new DebitApplicationRequest.DebitApplicationRequestData
            {
                NumberOfApprentices = request.NumberOfApprentices,
                Amount = request.Amount
            };

            var response = await _levyTransferMatchingService.DebitApplication(new DebitApplicationRequest(request.ApplicationId, data));

            return new DebitApplicationCommandResult
            {
                ErrorContent = response.ErrorContent,
                StatusCode = response.StatusCode
            };
        }
    }
}
