using System;
using SFA.DAS.Approvals.Application.Shared.Enums;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadUpdateLogRequest
    {
        public long? ProviderId { get; set; }
        public long? LogId { get; set; }
    }
}
