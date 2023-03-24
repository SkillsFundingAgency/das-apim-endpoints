using System.Web;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpOversight.InnerApi.Requests;

public class CreateProviderRequest : IPostApiRequest
{
    private readonly string _userId;
    private readonly string _userDisplayName;

    public string PostUrl => $"providers?userId={HttpUtility.UrlEncode(_userId)}&userDisplayName={HttpUtility.UrlEncode(_userDisplayName)}";
    public object Data { get; set; }
    public CreateProviderRequest(CreateProviderCommand data)
    {
        _userId = data.UserId;
        _userDisplayName = data.UserDisplayName;
        Data = data;
    }
}
