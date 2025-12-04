using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;
public class PostProviderRequest : IPostApiRequest
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string PostUrl => $"providers?userId={UserId}&userDisplayName={UserDisplayName}";

    public object Data { get; set; }
    public PostProviderRequest(CreateProviderModel data)
    {
        UserId = data.UserId;
        UserDisplayName = data.UserDisplayName;
        Data = data;
    }

}
