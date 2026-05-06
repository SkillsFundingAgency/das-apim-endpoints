using InnerChangeUserStatusResponse = SFA.DAS.ToolsSupport.InnerApi.Responses.ChangeUserStatusResponse;

namespace SFA.DAS.ToolsSupport.Api.Models.Users;

public class ChangeUserStatusResponse
{
    public string? Id { get; set; }
    public IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

    public static ChangeUserStatusResponse? FromInnerResponse(InnerChangeUserStatusResponse response)
    {
        if (response == null)
        {
            return null;
        }

        return new ChangeUserStatusResponse
        {
            Id = response.Id,
            Errors = response.Errors
        };
    }
}

