using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderQuery : IRequest<GetCourseProviderQueryResult>
{
    public long Ukprn { get; private set; }
    public string LarsCode { get; private set; }
    public string LocationName { get; private set; }
    public Guid ShortlistUserId { get; private set; }
    public int? Distance { get; private set; }

    public GetCourseProviderQuery(long ukprn, string larsCode, Guid shortlistUserId, string locationName, int? distance)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        LocationName = locationName;
        ShortlistUserId = shortlistUserId;
        Distance = distance;
    }
}
