using MediatR;

namespace SFA.DAS.Earnings.Application.LearnerData.GetLearnerData;

public class GetLearnerDataQuery(long ukprn, int academicYear, int page, int pageSize)
    : IRequest<GetLearnerDataQueryResult>
{
    public long Ukprn { get; } = ukprn;
    public int AcademicYear { get; } = academicYear;
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;
}