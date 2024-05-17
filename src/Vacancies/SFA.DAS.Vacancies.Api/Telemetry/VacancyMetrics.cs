using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace SFA.DAS.Vacancies.Api.Telemetry
{
    public class VacancyMetrics
    {
        private Counter<long> VacancyViewsCounter { get; }

        public VacancyMetrics(IMeterFactory meterFactory)
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
