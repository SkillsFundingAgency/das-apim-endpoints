namespace SFA.DAS.Earnings.Api.Learnerdata;

public interface ILearnerDataStore
{
    List<long> Search(long ukprn, int academicYear, int page, int pageSize);
    int Count(long ukprn, int academicYear);
}