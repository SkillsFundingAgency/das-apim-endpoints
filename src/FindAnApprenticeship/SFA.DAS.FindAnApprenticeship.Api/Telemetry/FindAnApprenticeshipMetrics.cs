using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.Telemetry
{
    public class FindAnApprenticeshipMetrics : IMetrics
    {
        private Counter<long> VacancyViewsCounter { get; }

        public FindAnApprenticeshipMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(Constants.OpenTelemetry.ServiceMeterName);
            VacancyViewsCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySearchViewsCounterName, "vacancy");
        }

        public void IncreaseVacancyViews(string vacancyReference, int viewCount = 1)
        {
            VacancyViewsCounter.Add(viewCount, 
                new KeyValuePair<string, object>("vacancy.reference", vacancyReference),
                new KeyValuePair<string, object>("vacancy.source", Constants.OpenTelemetry.RequestSourceName),
                new KeyValuePair<string, object>("vacancy.viewDate", DateTime.UtcNow));
        }
    }
}
