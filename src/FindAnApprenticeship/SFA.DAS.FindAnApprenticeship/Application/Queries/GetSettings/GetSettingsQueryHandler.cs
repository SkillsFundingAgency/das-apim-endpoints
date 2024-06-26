﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetSettings;

public class GetSettingsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient) : IRequestHandler<GetSettingsQuery, GetSettingsQueryResult>
{
    public async Task<GetSettingsQueryResult> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var postRequest = new PutCandidateApiRequest(request.CandidateId, new PutCandidateApiRequestData
        {
            Email = request.Email
        });

        await apiClient.PutWithResponseCode<PutCandidateApiResponse>(postRequest);
        var candidateTask = apiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId.ToString()));
        var preferencesTask = apiClient.Get<GetCandidatePreferencesApiResponse>(new GetCandidatePreferencesApiRequest(request.CandidateId));
        var aboutYouTask = apiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(request.CandidateId));

        await Task.WhenAll(candidateTask, preferencesTask,  aboutYouTask);

        var candidate = candidateTask.Result;
        var preferences = preferencesTask.Result;
        var address = candidate.Address;
        var aboutYou = aboutYouTask.Result;

        address ??= new GetAddressApiResponse();

        return new GetSettingsQueryResult
        {
            FirstName = candidate.FirstName,
            MiddleNames = candidate.MiddleNames,
            LastName = candidate.LastName,
            DateOfBirth = candidate.DateOfBirth,
            Uprn = address.Uprn,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            Town = address.Town,
            County = address.County,
            Postcode = address.Postcode,
            PhoneNumber = candidate.PhoneNumber,
            Email = candidate.Email,
            HasAnsweredEqualityQuestions = aboutYou?.AboutYou != null,
            CandidatePreferences = preferences.CandidatePreferences == null
                ? new List<GetSettingsQueryResult.CandidatePreference>()
                : preferences.CandidatePreferences.Select(cp => new GetSettingsQueryResult.CandidatePreference
                {
                    PreferenceId = cp.PreferenceId,
                    PreferenceMeaning = cp.PreferenceMeaning,
                    PreferenceHint = cp.PreferenceHint,
                    ContactMethodsAndStatus = cp.ContactMethodsAndStatus.Select(c => new GetSettingsQueryResult.ContactMethodStatus
                    {
                        ContactMethod = c.ContactMethod,
                        Status = c.Status
                    }).ToList()
                }).ToList()
        };
    }
}