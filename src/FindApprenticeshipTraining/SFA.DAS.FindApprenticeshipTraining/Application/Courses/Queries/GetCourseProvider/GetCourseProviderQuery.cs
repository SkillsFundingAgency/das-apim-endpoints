using MediatR;
using System;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderQuery : IRequest<GetCourseProviderQueryResult>
{
    public long Ukprn { get; private set; }
    public int LarsCode { get; private set; }
    public string Location { get; private set; }
    public Guid ShortlistUserId { get; private set; }
    public int? Distance { get; private set; }

    public GetCourseProviderQuery(long ukprn, int larsCode, Guid shortlistUserId, string location, int? distance)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        Location = location;
        ShortlistUserId = shortlistUserId;
        Distance = distance;
    }
}
