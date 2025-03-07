namespace SFA.DAS.Earnings.Application.ApprovedApprenticeships;

public interface IApprovedApprenticeshipsStore
{
    List<long> Search(long ukprn, int academicYear, int page, int pageSize);
    int Count(long ukprn, int academicYear);
}