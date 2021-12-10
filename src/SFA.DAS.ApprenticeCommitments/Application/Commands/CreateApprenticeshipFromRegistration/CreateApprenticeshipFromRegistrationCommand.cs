﻿using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands
{
    public class CreateApprenticeshipFromRegistration : IRequestHandler<CreateApprenticeshipFromRegistration.Command>
    {
        public class Command : IRequest
        {
            public Guid RegistrationId { get; set; }
            public Guid ApprenticeId { get; set; }
        }

        public CreateApprenticeshipFromRegistration(
            IInternalApiClient<ApprenticeAccountsConfiguration> accounts,
            IInternalApiClient<ApprenticeCommitmentsConfiguration> cmad,
            ILogger<Command> logger)
        {
            _cmad = cmad;
            _accounts = accounts;
            _logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateApprenticeshipFromRegistration Apprentice: {ApprenticeId} Registration: {RegistrationId}",
                request.ApprenticeId, request.RegistrationId);

            var apprentice = await _accounts.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));

            _logger.LogInformation("CreateApprenticeshipFromRegistration found apprentice {ApprenticeId}", apprentice.ApprenticeId);

            var response = await _cmad.PostWithResponseCode<object>(new CreateApprenticeshipRequest(
                request.RegistrationId, request.ApprenticeId, apprentice.LastName, apprentice.DateOfBirth));

            _logger.LogInformation("CreateApprenticeshipFromRegistration match returned HTTP {HttpStatus}", response.StatusCode);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }

        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _cmad;
        private readonly IInternalApiClient<ApprenticeAccountsConfiguration> _accounts;
        private readonly ILogger<Command> _logger;
    }
}