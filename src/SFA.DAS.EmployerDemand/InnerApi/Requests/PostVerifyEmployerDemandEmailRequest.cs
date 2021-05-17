﻿using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostVerifyEmployerDemandEmailRequest : IPostApiRequest<object>
    {
        private readonly Guid _id;

        public PostVerifyEmployerDemandEmailRequest(Guid id)
        {
            _id = id;
        }

        public string PostUrl => $"demand/{_id}/verify-email";
        public object Data { get; set; }
    }
}