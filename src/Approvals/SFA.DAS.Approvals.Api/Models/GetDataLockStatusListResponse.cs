using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetDataLockStatusListResponse
    {
        public int TotalNumberOfPages { get; set; }
        public IEnumerable<GetDataLockStatusResponse> DataLockStatuses { get; set; }
    }
}