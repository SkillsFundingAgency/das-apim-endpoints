using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddDraftApprenticeshipsRequest : IPostApiRequest
    {
        public string PostUrl => $"api/{ProviderId}/bulkupload";

        public PostAddDraftApprenticeshipsRequest(long providerId, BulkUploadAddDraftApprenticeshipsRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }

        public long ProviderId { get; }
        public object Data { get; set; }
    }
}
