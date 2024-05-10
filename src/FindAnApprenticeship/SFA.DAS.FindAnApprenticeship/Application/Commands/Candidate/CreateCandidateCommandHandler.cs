using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

    public CreateCandidateCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient,
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
    {
        _candidateApiClient = candidateApiClient;
        _legacyApiClient = legacyApiClient;
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
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
        var legacyApplications =
            await _legacyApiClient.Get<GetLegacyApplicationsByEmailApiResponse>(
                new GetLegacyApplicationsByEmailApiRequest(emailAddress));

        foreach (var legacyApplication in legacyApplications.Applications)
        {
            var vacancy = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(
                    new GetVacancyRequest(legacyApplication.Vacancy.VacancyReference));

            var additionalQuestions = new List<string>();
            if (vacancy.AdditionalQuestion1 != null) { additionalQuestions.Add(vacancy.AdditionalQuestion1); }
            if (vacancy.AdditionalQuestion2 != null) { additionalQuestions.Add(vacancy.AdditionalQuestion2); }

            PutApplicationApiRequest.PutApplicationApiRequestData putApplicationApiRequestData = new PutApplicationApiRequest.PutApplicationApiRequestData
            {
                CandidateId = candidateId,
                AdditionalQuestions = additionalQuestions,
                IsAdditionalQuestion1Complete = string.IsNullOrEmpty(vacancy.AdditionalQuestion1) ? (short)4 : (short)0,
                IsAdditionalQuestion2Complete = string.IsNullOrEmpty(vacancy.AdditionalQuestion2) ? (short)4 : (short)0,
                IsDisabilityConfidenceComplete = vacancy.IsDisabilityConfident ? (short)0 : (short)4
            };
            var putData = putApplicationApiRequestData;
            var vacancyReference =
                legacyApplication.Vacancy.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
            var putRequest = new PutApplicationApiRequest(vacancyReference, putData);
            var applicationResult =
                await _candidateApiClient.PutWithResponseCode<PutApplicationApiResponse>(putRequest);

            applicationResult.EnsureSuccessStatusCode();
        }
    }

    
}