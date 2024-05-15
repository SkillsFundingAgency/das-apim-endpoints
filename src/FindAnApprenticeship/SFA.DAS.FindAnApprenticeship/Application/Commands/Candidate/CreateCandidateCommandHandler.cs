using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

public class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, CreateCandidateCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> _legacyApiClient;
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ILogger<CreateCandidateCommandHandler> _logger;

    public CreateCandidateCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient,
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ILogger<CreateCandidateCommandHandler> logger)
    {
        _candidateApiClient = candidateApiClient;
        _legacyApiClient = legacyApiClient;
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _logger = logger;
    }

    public async Task<CreateCandidateCommandResult> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var existingUser =
            await _candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                new GetCandidateApiRequest(request.GovUkIdentifier));

        if (existingUser.StatusCode != HttpStatusCode.NotFound)
        {
            return new CreateCandidateCommandResult
            {
                Id = existingUser.Body.Id,
                GovUkIdentifier = existingUser.Body.GovUkIdentifier,
                Email = existingUser.Body.Email,
                FirstName = existingUser.Body.FirstName,
                LastName = existingUser.Body.LastName,
                PhoneNumber = existingUser.Body.PhoneNumber,
                DateOfBirth = existingUser.Body.DateOfBirth,
                Status = existingUser.Body.Status
            };
        }

        var userDetails =
            await _legacyApiClient.Get<GetLegacyUserByEmailApiResponse>(
                new GetLegacyUserByEmailApiRequest(request.Email));

        var registrationDetailsDateOfBirth = userDetails?.RegistrationDetails?.DateOfBirth;
        if (registrationDetailsDateOfBirth == DateTime.MinValue)
        {
            registrationDetailsDateOfBirth = null;
        }

        var postData = new PostCandidateApiRequestData
        {
            Email = request.Email,
            FirstName = userDetails?.RegistrationDetails?.FirstName,
            LastName = userDetails?.RegistrationDetails?.LastName,
            DateOfBirth = registrationDetailsDateOfBirth,
        };

        var postRequest = new PostCandidateApiRequest(request.GovUkIdentifier, postData);

        var candidateResult = await _candidateApiClient.PostWithResponseCode<PostCandidateApiResponse>(postRequest);

        candidateResult.EnsureSuccessStatusCode();

        if (candidateResult is null) return null;

        await MigrateLegacyApplications(candidateResult.Body.Id, request.Email);

        return new CreateCandidateCommandResult
        {
            Id = candidateResult.Body.Id,
            GovUkIdentifier = candidateResult.Body.GovUkIdentifier,
            Email = candidateResult.Body.Email,
            FirstName = candidateResult.Body.FirstName,
            LastName = candidateResult.Body.LastName,
            PhoneNumber = candidateResult.Body.PhoneNumber,
            DateOfBirth = registrationDetailsDateOfBirth,
            Status = UserStatus.Incomplete
        };
    }

    private async Task MigrateLegacyApplications(Guid candidateId, string emailAddress)
    {
        _logger.LogInformation($"Migrating applications for candidate [{candidateId}] using email address [{emailAddress}].");

        var legacyApplications =
            await _legacyApiClient.Get<GetLegacyApplicationsByEmailApiResponse>(
                new GetLegacyApplicationsByEmailApiRequest(emailAddress));

        if (legacyApplications?.Applications == null || legacyApplications.Applications.Count == 0)
        {
            _logger.LogInformation($"No legacy applications found for email address [{emailAddress}].");
            return;
        }

        foreach (var legacyApplication in legacyApplications.Applications)
        {
            var vacancy = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(
                    new GetVacancyRequest(legacyApplication.Vacancy.VacancyReference));

            if (vacancy == null)
            {
                _logger.LogError($"Unable to retrieve vacancy [{legacyApplication.Vacancy.VacancyReference}].");
                continue;
            }

            var data = new PostApplicationApiRequest.PostApplicationApiRequestData
            {
                LegacyApplication = PostApplicationApiRequest.LegacyApplication.Map(legacyApplication, vacancy, candidateId)
            };
            var postRequest = new PostApplicationApiRequest(data);

            var applicationResult =
                await _candidateApiClient.PostWithResponseCode<PostApplicationApiResponse>(postRequest);

            applicationResult.EnsureSuccessStatusCode();
        }
    }
}