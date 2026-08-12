using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;

namespace SFA.DAS.Admin.Api.Models.UserActions
{
    public class CheckUserActionByCodeRequest
    {
        public string Username { get; set; }

        public static implicit operator CheckUserActionByCodeCommand(CheckUserActionByCodeRequest request)
        {
            if (request == null) return new CheckUserActionByCodeCommand();
            return new CheckUserActionByCodeCommand
            {
                Username = request.Username
            };
        }
    }
}
