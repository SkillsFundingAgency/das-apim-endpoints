using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Domain
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public static class SearchApprenticeships
        {
            public static readonly int DefaultPageNumber = 1;
            public static readonly int DefaultPageSize = 10;
            public static readonly VacancySort DefaultSortOrder = VacancySort.DistanceAsc;
        }

        public static class SectionStatus
        {
            public static readonly string NotYetStarted = "NotYetStarted";
        }
    }
}
