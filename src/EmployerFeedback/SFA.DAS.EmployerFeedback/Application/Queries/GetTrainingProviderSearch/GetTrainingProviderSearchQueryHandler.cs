using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerFeedback.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetTrainingProviderSearch
{
    public class GetTrainingProviderSearchQueryHandler : IRequestHandler<GetTrainingProviderSearchQuery, GetTrainingProviderSearchResult>
    {
        public const string RoatpProvidersCacheKey = "GetTrainingProviderSearchQueryHandler.Providers";

        private readonly ICacheStorageService _cacheStorageService;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly IRoatpV2TrainingProviderService _roatpV2TrainingProviderService;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly EmployerFeedbackConfiguration _employerFeedbackConfiguration;

        public GetTrainingProviderSearchQueryHandler(ICacheStorageService cacheStorageService,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
            IRoatpV2TrainingProviderService roatpV2TrainingProviderService,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            IOptions<EmployerFeedbackConfiguration> options)
        {
            _cacheStorageService = cacheStorageService;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _roatpV2TrainingProviderService = roatpV2TrainingProviderService;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _employerFeedbackConfiguration = options.Value;
        }

        public async Task<GetTrainingProviderSearchResult> Handle(GetTrainingProviderSearchQuery request, CancellationToken cancellationToken)
        {
            var activeMainProvidersTask = GetActiveMainProvidersAsync(cancellationToken);

            var providersCourseStatusTask = GetProvidersCourseStatusAsync(
                request.AccountId,
                _employerFeedbackConfiguration.AccountProvidersCourseStatusCompletionLag,
                _employerFeedbackConfiguration.AccountProvidersCourseStatusStartLag,
                _employerFeedbackConfiguration.AccountProvidersCourseStatusNewStartWindow,
                cancellationToken);

            var latestEmployerFeedbackTask = GetLatestEmployerFeedbackAsync(request.AccountId, request.UserRef, cancellationToken);

            await Task.WhenAll(activeMainProvidersTask, providersCourseStatusTask, latestEmployerFeedbackTask);

            var activeMainProviders = await activeMainProvidersTask ?? new List<Provider>();
            var accountProvidersCourseStatus = await providersCourseStatusTask ?? new GetAccountProvidersCourseStatusResponse();
            var latestEmployerFeedback = await latestEmployerFeedbackTask ?? new GetLatestEmployerFeedbackResponse { EmployerFeedbacks = new List<EmployerFeedbackItem>() };

            var providersWithNewStart = new HashSet<long>(accountProvidersCourseStatus.NewStart?.Select(x => x.Ukprn) ?? Enumerable.Empty<long>());
            var providersWithActive = new HashSet<long>(accountProvidersCourseStatus.Active?.Select(x => x.Ukprn) ?? Enumerable.Empty<long>());
            var providersWithCompleted = new HashSet<long>(accountProvidersCourseStatus.Completed?.Select(x => x.Ukprn) ?? Enumerable.Empty<long>());

            var accountProviders = new HashSet<long>(providersWithNewStart);
            accountProviders.UnionWith(providersWithActive);
            accountProviders.UnionWith(providersWithCompleted);

            var providerNameByUkprn = activeMainProviders
                .GroupBy(p => (long)p.Ukprn)
                .ToDictionary(g => g.Key, g => g.First().Name);

            var latestFeedbackByUkprn = (latestEmployerFeedback.EmployerFeedbacks ?? new List<EmployerFeedbackItem>())
                .GroupBy(f => f.Ukprn)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.Result?.DateTimeCompleted ?? DateTime.MinValue)
                          .First().Result);

            var providers = accountProviders
                .Where(ukprn => providerNameByUkprn.ContainsKey(ukprn))
                .Select(ukprn =>
                {
                    latestFeedbackByUkprn.TryGetValue(ukprn, out var feedback);
                    providerNameByUkprn.TryGetValue(ukprn, out var providerName);

                    return new TrainingProviderSearchResult
                    {
                        Ukprn = ukprn,
                        ProviderName = providerName ?? string.Empty,
                        HasNewStart = providersWithNewStart.Contains(ukprn),
                        HasActive = providersWithActive.Contains(ukprn),
                        HasCompleted = providersWithCompleted.Contains(ukprn),
                        Feedback = feedback != null ? new TrainingProviderFeedback
                        {
                            DateTimeCompleted = feedback.DateTimeCompleted,
                            ProviderRating = feedback.ProviderRating,
                            FeedbackSource = feedback.FeedbackSource
                        } : null
                    };
                })
                .OrderBy(x => x.ProviderName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new GetTrainingProviderSearchResult
            {
                AccountId = latestEmployerFeedback.AccountId,
                AccountName = latestEmployerFeedback.AccountName,
                Providers = providers
            };
        }

        private async Task<List<Provider>> GetActiveMainProvidersAsync(CancellationToken cancellationToken)
        {
            var cachedResponse = await _cacheStorageService.RetrieveFromCache<List<Provider>>(RoatpProvidersCacheKey);
            if (cachedResponse != null) return cachedResponse;

            try
            {
                var roatpProvidersResponse = await _roatpV2TrainingProviderService.GetProviders(true);
                var providers = roatpProvidersResponse?.RegisteredProviders?.Where(p => p.ProviderTypeId == 1 && (p.StatusId == 1 || p.StatusId == 2)).ToList() ?? [];
                await _cacheStorageService.SaveToCache(RoatpProvidersCacheKey, providers, 4);
                return providers;
            }
            catch (Exception)
            {
                return [];
            }
        }

        private Task<GetAccountProvidersCourseStatusResponse> GetProvidersCourseStatusAsync(long accountId, int completionLag, int startLag, int newStartWindow, CancellationToken cancellationToken)
        {
            return _commitmentsV2ApiClient.Get<GetAccountProvidersCourseStatusResponse>(
                new GetAccountProvidersCourseStatusRequest(accountId, completionLag, startLag, newStartWindow));
        }

        private Task<GetLatestEmployerFeedbackResponse> GetLatestEmployerFeedbackAsync(long accountId, Guid userRef, CancellationToken cancellationToken)
        {
            return _employerFeedbackApiClient.Get<GetLatestEmployerFeedbackResponse>(
                new GetLatestEmployerFeedbackRequest(accountId, userRef));
        }
    }
}
