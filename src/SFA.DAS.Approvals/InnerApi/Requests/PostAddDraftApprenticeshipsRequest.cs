using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddDraftApprenticeshipsRequest : IPostApiRequest
    {
        private readonly long _providerId;
        public object Data { get; set; }
        public string PostUrl => $"api/{_providerId}/bulkupload";

        public PostAddDraftApprenticeshipsRequest(long providerId, BulkUploadAddDraftApprenticeshipsRequest data)
        {
            _providerId = providerId;
            Data = data;
        }
    }
}
