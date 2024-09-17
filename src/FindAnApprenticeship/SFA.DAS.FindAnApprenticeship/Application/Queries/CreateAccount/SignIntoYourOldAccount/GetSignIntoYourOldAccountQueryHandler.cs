using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount;

public class GetSignIntoYourOldAccountQueryHandler(
    IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient,
    ICacheStorageService cacheStorageService,
    IDateTimeService dateTimeService)
    : IRequestHandler<GetSignIntoYourOldAccountQuery, GetSignIntoYourOldAccountQueryResult>
{
    public async Task<GetSignIntoYourOldAccountQueryResult> Handle(GetSignIntoYourOldAccountQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(request.CandidateId);
        var cacheItem = await cacheStorageService.RetrieveFromCache<SignInAttemptHistory>(cacheKey);

        if (cacheItem != null)
        {
            cacheItem.SignInAttempts.RemoveAll(x => x < dateTimeService.UtcNow.AddMinutes(-30));

            if (cacheItem.SignInAttempts.Count >= 5)
            {
                return new GetSignIntoYourOldAccountQueryResult { IsValid = false };
            }
        }

        var result =
            await legacyApiClient.PostWithResponseCode<PostLegacyValidateCredentialsApiResponse>(
                new PostLegacyValidateUserCredentialsApiRequest(
                    new PostLegacyValidateUserCredentialsApiRequestBody
                    {
                        Email = request.Email,
                        Password = request.Password
                    }));

        result.EnsureSuccessStatusCode();

        if (result is null) return new GetSignIntoYourOldAccountQueryResult
        {
            IsValid = false
        };

        if (!result.Body.IsValid)
        {
            cacheItem ??= new SignInAttemptHistory();
            cacheItem.SignInAttempts.Add(dateTimeService.UtcNow);
            await cacheStorageService.SaveToCache(cacheKey, cacheItem, 1);
        }
        else if(cacheItem != null)
        {
            await cacheStorageService.DeleteFromCache(cacheKey);
        }

        return new GetSignIntoYourOldAccountQueryResult
        {
            IsValid = result.Body.IsValid
        };
    }

    public class SignInAttemptHistory
    {
        public List<DateTime> SignInAttempts { get; set; } = [];
    }

    public string GetCacheKey(Guid candidateId)
    {
        return $"{nameof(SignInAttemptHistory)}-{candidateId}";
    }
}