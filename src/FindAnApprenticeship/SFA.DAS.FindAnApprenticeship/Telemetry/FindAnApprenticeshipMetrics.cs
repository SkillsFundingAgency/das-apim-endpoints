using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Extensions;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace SFA.DAS.FindAnApprenticeship.Telemetry
{
    public class FindAnApprenticeshipMetrics : IMetrics
    {
        private Counter<long> VacancyViewsCounter { get; }
        private Counter<long> VacancyStartedCounter { get; }
        private Counter<long> VacancySubmittedCounter { get; }
        private Counter<long> VacancySearchResultViewCounter { get; }

        public FindAnApprenticeshipMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(Constants.OpenTelemetry.ServiceMeterName);
            VacancyViewsCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySearchViewsCounterName, Constants.OpenTelemetry.CounterUnitName, "Instrument to calculate the vacancy views");
            VacancyStartedCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancyStartedCounterName, Constants.OpenTelemetry.CounterUnitName, "Instrument to calculate the vacancy application started");
            VacancySubmittedCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySubmittedCounterName, Constants.OpenTelemetry.CounterUnitName, "Instrument to calculate the vacancy application submitted");
            VacancySearchResultViewCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySearchResultCounterName, Constants.OpenTelemetry.CounterUnitName, "Instrument to calculate the vacancy appeared in the search results");
        }

        public void IncreaseVacancyViews(string vacancyReference, int viewCount = 1)
        {
            VacancyViewsCounter.Add(viewCount,
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference.TrimVacancyReference()),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName));
        }

        public void IncreaseVacancyStarted(string vacancyReference, int viewCount = 1)
        {
            VacancyStartedCounter.Add(viewCount,
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference.TrimVacancyReference()),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName));
        }

        public void IncreaseVacancySubmitted(string vacancyReference, int viewCount = 1)
        {
            VacancySubmittedCounter.Add(viewCount,
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference.TrimVacancyReference()),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName));
        }

        public void IncreaseVacancySearchResultViews(string vacancyReference, int viewCount = 1)
        {
            VacancySearchResultViewCounter.Add(viewCount,
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference.TrimVacancyReference()),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName));
        }
    }
}