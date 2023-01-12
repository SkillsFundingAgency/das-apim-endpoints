using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class DeleteProviderCourseLocationRequest : IDeleteApiRequest
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string DeleteUrl => $"/providers/{Ukprn}/courses/{LarsCode}/location/{Id}?userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
    }
}
