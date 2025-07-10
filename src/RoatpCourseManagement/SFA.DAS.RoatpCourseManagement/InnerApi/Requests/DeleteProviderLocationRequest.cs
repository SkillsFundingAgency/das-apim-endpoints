using System;
using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class DeleteProviderLocationRequest : IDeleteApiRequest
{
    public int Ukprn { get; set; }
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }

    public string DeleteUrl => $"/providers/{Ukprn}/locations/{Id}?userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
}