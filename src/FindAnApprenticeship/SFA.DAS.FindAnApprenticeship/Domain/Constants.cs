using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Domain
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public static class SearchApprenticeships
        {
            public const int DefaultPageNumber = 1;
            public const int DefaultPageSize = 10;
            public const VacancySort DefaultSortOrder = VacancySort.DistanceAsc;
        }

        public static class SectionStatus
        {
            public const string NotStarted = "NotStarted";
            public const string InProgress = "InProgress";
			public const string Incomplete = "Incomplete";
            public const string Completed = "Completed";
        }

        public static class OpenTelemetry
        {
            public const string ServiceName = "FindAnApprenticeshipOuterApi";
            public const string ServiceMeterName = "FindAnApprenticeship";
            public const string RequestSourceName = "faav2ui-as";
            public const string CounterUnitName = "vacancy";
            public const string VacancySearchViewsCounterName = "FindAnApprenticeship.vacancyReference.views";
            public const string VacancyStartedCounterName = "FindAnApprenticeship.vacancyReference.started";
            public const string VacancySubmittedCounterName = "FindAnApprenticeship.vacancyReference.submitted";
            public const string VacancySearchResultCounterName = "FindAnApprenticeship.vacancyReference.search";
            public const string VacancySavedCounterName = "FindAnApprenticeship.vacancyReference.saved";
        }
    }
}