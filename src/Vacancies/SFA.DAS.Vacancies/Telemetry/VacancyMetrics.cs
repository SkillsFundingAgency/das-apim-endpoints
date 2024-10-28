using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using SFA.DAS.Vacancies.Services;

namespace SFA.DAS.Vacancies.Telemetry
{
    public class VacancyMetrics : IMetrics
    {
        private Counter<long> VacancyViewsCounter { get; }
        private Counter<long> VacancySearchResultViewCounter { get; }

        public VacancyMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(Constants.OpenTelemetry.ServiceMeterName);
            VacancyViewsCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySearchViewsCounterName, Constants.OpenTelemetry.CounterUnitName, "Instrument to calculate the vacancy views");
            VacancySearchResultViewCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySearchResultCounterName, Constants.OpenTelemetry.CounterUnitName, "Instrument to calculate the vacancy appeared in the search results");
        }

        public void IncreaseVacancyViews(string vacancyReference, int viewCount = 1)
        {
            VacancyViewsCounter.Add(viewCount,
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference.Replace("VAC", string.Empty, StringComparison.CurrentCultureIgnoreCase)),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName));
        }

        public void IncreaseVacancySearchResultViews(string vacancyReference, int viewCount = 1)
        {
            VacancySearchResultViewCounter.Add(viewCount,
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference.Replace("VAC", string.Empty, StringComparison.CurrentCultureIgnoreCase)),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName));
        }
    }
    
    public class MockVacancyMetrics : IMetrics
    {
        public void IncreaseVacancyViews(string vacancyReference, int viewCount = 1)
        {
            
        }

        public void IncreaseVacancySearchResultViews(string vacancyReference, int viewCount = 1)
        {
            
        }
    }
}
