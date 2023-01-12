using SFA.DAS.Approvals.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetDataLockStatusListResponse
    {
        public IEnumerable<GetDataLockStatusResponse> DataLockStatuses { get; set; }
    }
}