using System;
using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class DeleteProviderCourseLocationRequest : IDeleteApiRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public Guid ProviderCourseLocationId { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string DeleteUrl => $"/providers/{Ukprn}/courses/{LarsCode}/locations/{ProviderCourseLocationId}?userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
}
