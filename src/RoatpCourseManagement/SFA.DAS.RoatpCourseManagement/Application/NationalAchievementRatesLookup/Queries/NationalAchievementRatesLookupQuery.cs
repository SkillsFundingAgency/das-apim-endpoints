using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries
{
    public class NationalAchievementRatesLookupQuery : IRequest<NationalAchievementRatesLookupQueryResult>
    {
        public int ForYear { get; }
        public NationalAchievementRatesLookupQuery(int forYear)
        {
            ForYear = forYear;
        }
    }
}
