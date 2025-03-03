using System;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQueryResult : PagedQueryResult<GetApplicationsQueryResult.Application>
    {
        public static implicit operator GetApplicationsQueryResult(GetApplicationsResponse applications)
        {
            return new GetApplicationsQueryResult
            {
                Items = applications.Applications.Select(x => (Application)x).ToList(),
                TotalItems = applications.TotalItems,
                PageSize = applications.PageSize,
                Page = applications.Page
            };
        }
        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
            public int PledgeId { get; set; }
            public string Details { get; set; }
            public int NumberOfApprentices { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsNamePublic { get; set; }
            public string Status { get; set; }
            public static implicit operator Application(GetApplicationsResponse.Application source)
            {
                return new Application
                {
                    Id = source.Id,
                    DasAccountName = source.DasAccountName,
                    PledgeId = source.PledgeId,
                    Details = source.Details,
                    NumberOfApprentices = source.NumberOfApprentices,
                    Amount = source.TotalAmount,
                    TotalAmount = source.TotalAmount,
                    CreatedOn = source.CreatedOn,
                    IsNamePublic = source.IsNamePublic,
                    Status = source.Status,
                };
            }
        }
    }
}