namespace SFA.DAS.Earnings.Api.Learnerdata;

public interface ILearnerDataSearchService
{
    PagedResult Search(uint ukprn, uint academicYear, uint page, uint pageSize);
}