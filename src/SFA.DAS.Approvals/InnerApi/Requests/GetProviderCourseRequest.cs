﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetProviderCourseRequest : IGetApiRequest
    {
        public long ProviderId { get; }
        public string CourseCode { get; }

        public GetProviderCourseRequest(long providerId, string courseCode)
        {
            ProviderId = providerId;
            CourseCode = courseCode;
        }
        public string GetUrl => $"providers/{ProviderId}/courses/{CourseCode}";
    }
}