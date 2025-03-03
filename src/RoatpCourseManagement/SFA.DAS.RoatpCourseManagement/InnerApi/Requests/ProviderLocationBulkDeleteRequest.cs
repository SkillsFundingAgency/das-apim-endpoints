﻿using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class ProviderLocationBulkDeleteRequest : IDeleteApiRequest
    {
        public int Ukprn { get; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string DeleteUrl => $"/providers/{Ukprn}/locations/cleanup?userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
        public ProviderLocationBulkDeleteRequest(ProviderLocationBulkDeleteModel data)
        {
            Ukprn = data.Ukprn;
            UserId = data.UserId;
            UserDisplayName = data.UserDisplayName;
        }
    }
}
