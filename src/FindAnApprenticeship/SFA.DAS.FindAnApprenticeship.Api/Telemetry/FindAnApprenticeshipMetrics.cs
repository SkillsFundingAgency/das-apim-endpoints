using System.Collections.Generic;
using System.Diagnostics.Metrics;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.Telemetry
{
    public class FindAnApprenticeshipMetrics
    {
        private Counter<long> VacancyViewsCounter { get; }

        public FindAnApprenticeshipMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(Constants.OpenTelemetry.ServiceMeterName);
            VacancyViewsCounter = meter.CreateCounter<long>(Constants.OpenTelemetry.VacancySearchViewsCounterName, "VacancyReference");
        }

        public void IncreaseVacancyViews(string vacancyReference)
        {
            VacancyViewsCounter.Add(1, 
                new KeyValuePair<string, object>("vacancyReference", vacancyReference),
                new KeyValuePair<string, object>("source", Constants.OpenTelemetry.RequestSourceName));
        }
    }
}
