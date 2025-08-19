using System.Web;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class CreateProviderContactRequest : IPostApiRequest
{
    public readonly int Ukprn;
    private readonly string UserId;
    private readonly string UserDisplayName;
    public string PostUrl => $"providers/{Ukprn}/contact?userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
    public object Data { get; set; }
    public CreateProviderContactRequest(CreateProviderContactCommand data)
    {
        Ukprn = data.Ukprn;
        UserId = data.UserId;
        UserDisplayName = data.UserDisplayName;
        Data = data;
    }
}