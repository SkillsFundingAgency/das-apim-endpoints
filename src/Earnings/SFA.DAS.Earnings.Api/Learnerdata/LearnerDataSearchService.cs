namespace SFA.DAS.Earnings.Api.Learnerdata;

public class LearnerDataSearchService : ILearnerDataSearchService
{
    private readonly ILearnerDataStore _datastore;

    public LearnerDataSearchService(ILearnerDataStore datastore)
    {
        _datastore = datastore;
    }
    
    public PagedResult Search(uint ukprn, uint academicYear, uint page, uint pageSize)
    {
        var results = _datastore.Search(ukprn, academicYear,  page, pageSize);

        var totalRecords = _datastore.Count(ukprn, academicYear);
        
        return new PagedResult
        {
            Page = page,
            PageSize = pageSize,
            TotalRecords = (uint)totalRecords,
            Apprenticeships = results,
            TotalPages = (uint)Math.Ceiling((double)totalRecords / pageSize)
        };
    }
    
}