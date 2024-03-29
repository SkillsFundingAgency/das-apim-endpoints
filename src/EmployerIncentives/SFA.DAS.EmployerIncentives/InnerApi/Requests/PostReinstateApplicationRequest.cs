﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostReinstateApplicationRequest : IPostApiRequest
    {
        public PostReinstateApplicationRequest(ReinstateApplicationRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"withdrawal-reinstatements";
        public object Data { get; set; }
    }
}
