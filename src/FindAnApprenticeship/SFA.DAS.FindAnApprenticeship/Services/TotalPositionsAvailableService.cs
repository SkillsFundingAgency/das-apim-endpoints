using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Services;

public class TotalPositionsAvailableService(
    IRecruitApiClient<RecruitApiV2Configuration> recruitApiV2Client,
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

        var raaVacanciesCountTask = recruitApiV2Client.Get<GetTotalPositionsAvailableResponse>(new GetTotalPositionsAvailableRequest());
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
                    VacancyDataSource.Csj,
                },
                null,
                null));

        await Task.WhenAll(raaVacanciesCountTask, externalVacanciesCountTask);
            
        var totalPositionsAvailable = raaVacanciesCountTask.Result.TotalPositionsAvailable + externalVacanciesCountTask.Result.TotalVacancies;

        await cacheStorageService.SaveToCache(nameof(GetTotalPositionsAvailableRequest), totalPositionsAvailable,
            TimeSpan.FromHours(1));

        return totalPositionsAvailable;
    }
}