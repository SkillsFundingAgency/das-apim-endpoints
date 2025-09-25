using MediatR;

namespace SFA.DAS.LearnerData.Application.Fm36;

public class GetFm36Command : IRequest<GetFm36Result>
{
    public GetFm36Command(long ukprn, int collectionYear, byte collectionPeriod)
    {
        Ukprn = ukprn;
        CollectionYear = collectionYear;
        CollectionPeriod = collectionPeriod;
    }

    public long Ukprn { get; }
    public int CollectionYear { get; }
    public byte CollectionPeriod { get; }
}
