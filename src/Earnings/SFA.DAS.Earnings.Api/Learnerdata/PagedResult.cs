using SFA.DAS.Earnings.Api.Controllers;

namespace SFA.DAS.Earnings.Api.Learnerdata;

public class PagedResult {
    public IList<Apprenticeship> Apprenticeships { get; set; }
    public uint TotalRecords { get; set; }   
    public uint Page { get; set; }   
    public uint PageSize { get; set; }   
    public uint TotalPages { get; set; }

    public PagedResult()
    {
        Apprenticeships = new List<Apprenticeship>();
    }
}