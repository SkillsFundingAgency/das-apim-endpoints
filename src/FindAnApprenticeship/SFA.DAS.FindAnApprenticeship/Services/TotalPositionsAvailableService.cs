using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Services;

public class TotalPositionsAvailableService(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICacheStorageService cacheStorageService) : ITotalPositionsAvailableService
{
    public async Task<long> GetTotalPositionsAvailable()
    {
        var cachedValue = await cacheStorageService.RetrieveFromCache<long?>(nameof(GetTotalPositionsAvailableRequest));

        if (cachedValue.HasValue)
        {
            return cachedValue.Value;
        }

        var raaVacanciesCountTask = recruitApiClient.Get<long>(new GetTotalPositionsAvailableRequest());
        var externalVacanciesCountTask = findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
            new GetApprenticeshipCountRequest(null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                false,
                new List<VacancyDataSource>
                {
                    VacancyDataSource.Nhs,
                },
                null,
                null));

        await Task.WhenAll(raaVacanciesCountTask, externalVacanciesCountTask);
            
        var totalPositionsAvailable = raaVacanciesCountTask.Result + externalVacanciesCountTask.Result.TotalVacancies;

        await cacheStorageService.SaveToCache(nameof(GetTotalPositionsAvailableRequest), totalPositionsAvailable,
            TimeSpan.FromHours(1));

        return totalPositionsAvailable;
    }
}