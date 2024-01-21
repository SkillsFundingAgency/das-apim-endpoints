using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication
{
    public class PatchApplicationCommandHandler :IRequestHandler<PatchApplicationCommand, PatchApplicationCommandResponse>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
        private readonly ILogger<PatchApplicationCommandHandler> _logger;

        public PatchApplicationCommandHandler(
            ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
            IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, ILogger<PatchApplicationCommandHandler> logger)
        {
            _candidateApiClient = candidateApiClient;
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _logger = logger;
        }

        public async Task<PatchApplicationCommandResponse> Handle(PatchApplicationCommand request, CancellationToken cancellationToken)
        {
            var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, request.Data);

            var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
            }

        }
    }
}
