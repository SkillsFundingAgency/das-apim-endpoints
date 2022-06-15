using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostReinstateApplicationRequest : IPostApiRequest<ReinstateApplicationRequest>
    {
        public PostReinstateApplicationRequest(ReinstateApplicationRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"withdrawal-reinstatements";
        public ReinstateApplicationRequest Data { get; set; }
    }
}
