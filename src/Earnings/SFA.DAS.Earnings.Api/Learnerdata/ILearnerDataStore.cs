using SFA.DAS.Earnings.Api.Controllers;

namespace SFA.DAS.Earnings.Api.Learnerdata;

public interface ILearnerDataStore
{
    List<Apprenticeship> Search(uint ukprn, uint academicYear, uint page, uint pageSize);
    int Count(uint ukprn, uint academicYear);
}