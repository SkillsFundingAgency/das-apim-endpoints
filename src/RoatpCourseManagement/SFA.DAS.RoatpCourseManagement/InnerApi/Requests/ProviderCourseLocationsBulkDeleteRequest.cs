﻿using SFA.DAS.RoatpCourseManagement.InnerApi.Models.DeleteProviderCourseLocations;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderCourseLocationsBulkDeleteRequest : IDeleteApiRequest
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public DeleteProviderCourseLocationOption DeleteProviderCourseLocationOption { get; set; }

        public string DeleteUrl => $"/providers/{Ukprn}/courses/{LarsCode}/locations?options={DeleteProviderCourseLocationOption}&userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
    }
}
