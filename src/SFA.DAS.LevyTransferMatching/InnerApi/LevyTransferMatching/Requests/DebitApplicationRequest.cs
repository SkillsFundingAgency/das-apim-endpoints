using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class DebitApplicationRequest : IPostApiRequest
    {
        private readonly int _applicationId;

        public DebitApplicationRequest(int applicationId, DebitApplicationRequestData data)
        {
            _applicationId = applicationId;
            Data = data;
        }

        public string PostUrl => $"applications/{_applicationId}/debit";

        public object Data { get; set; }

        public class DebitApplicationRequestData
        {
            public int NumberOfApprentices { get; set; }
            public int Amount { get; set; }
            public int MaxAmount { get; set; }
        }
    }
}
