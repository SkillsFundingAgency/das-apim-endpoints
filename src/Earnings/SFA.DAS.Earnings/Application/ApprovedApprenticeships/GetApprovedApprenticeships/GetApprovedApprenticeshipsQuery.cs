using MediatR;

namespace SFA.DAS.Earnings.Application.ApprovedApprenticeships.GetApprovedApprenticeships;

public class GetApprovedApprenticeshipsQuery(long ukprn, int academicYear, int page, int pageSize)
    : IRequest<GetApprovedApprenticeshipsQueryResult>
{
    public long Ukprn { get; } = ukprn;
    public int AcademicYear { get; } = academicYear;
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;
}