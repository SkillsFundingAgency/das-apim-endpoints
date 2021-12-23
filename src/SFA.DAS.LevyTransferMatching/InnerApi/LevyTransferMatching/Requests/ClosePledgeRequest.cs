using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class ClosePledgeRequest : IPostApiRequest
    {
        public int _pledgeId { get; set; }
        public string PostUrl => $"pledges/{_pledgeId}/close";

        public ClosePledgeRequest(int pledgeId, ClosePledgeRequestData data)
        {
            _pledgeId = pledgeId;
            Data = data;

        }
        public object Data { get; set; }

        public class ClosePledgeRequestData
        {
            public int Status { get; set; }

            public ClosePledgeRequestData(int status){
                Status = status;
            }

        }
    }
}
