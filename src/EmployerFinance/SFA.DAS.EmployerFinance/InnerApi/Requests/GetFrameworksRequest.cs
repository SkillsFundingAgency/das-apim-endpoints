﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests
{
    public class GetFrameworksRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/frameworks";
    }
}