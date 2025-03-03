﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetProviderRequest : IGetApiRequest
    {
        public string GetUrl => $"providers/{Ukprn}";
        public int Ukprn { get; }
     
        public GetProviderRequest(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
