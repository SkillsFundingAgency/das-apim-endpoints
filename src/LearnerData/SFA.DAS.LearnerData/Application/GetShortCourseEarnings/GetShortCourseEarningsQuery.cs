using MediatR;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsQuery : PagedQuery, IRequest<GetShortCourseEarningsQueryResult>
{
    public GetShortCourseEarningsQuery(long ukprn, int collectionYear, byte collectionPeriod, int? page, int? pageSize)
    {
        Ukprn = ukprn;
        CollectionYear = collectionYear;
        CollectionPeriod = collectionPeriod;

        Page = page ?? -1;
        PageSize = pageSize;
    }

    public long Ukprn { get; }
    public int CollectionYear { get; }
    public byte CollectionPeriod { get; }
    public bool IsPaged => Page > 0 && PageSize.HasValue;
}
