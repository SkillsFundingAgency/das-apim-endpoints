using SFA.DAS.Admin.Application.Commands.UnlockUser;

namespace SFA.DAS.Admin.Api.Models.Users
{
    public class UnlockUserRequest
    {
        public required string Username { get; set; }
        public long UserActionId { get; set; }

        public static implicit operator UnlockUserCommand(UnlockUserRequest request)
        {
            if (request == null) return new UnlockUserCommand();
            return new UnlockUserCommand
            {
                Username = request.Username,
                UserActionId = request.UserActionId
            };
        }
    }
}
