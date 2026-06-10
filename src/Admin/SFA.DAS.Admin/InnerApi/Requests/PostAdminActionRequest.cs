using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Admin.InnerApi.Requests
{
    public class PostAdminActionRequest : IPostApiRequest<PostAdminActionRequestData>
    {
        public PostAdminActionRequestData Data { get; set; }

        public PostAdminActionRequest(string username, string action, long userActionId)
        {
            Data = new PostAdminActionRequestData
            {
                Username = username,
                Action = action,
                UserActionId = userActionId
            };
        }

        public string PostUrl => "api/users/adminactions";
    }

    public class PostAdminActionRequestData
    {
        public string Username { get; set; }
        public string Action { get; set; }
        public long UserActionId { get; set; }
    }
}
