﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    class GetProviderCourseLocationsRequest : IGetApiRequest
    {
        public string GetUrl => $"provider/{Ukprn}/courses/{LarsCode}/locations";
        public int Ukprn { get; }
        public int LarsCode { get; }

        public GetProviderCourseLocationsRequest(int ukprn, int larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}

