using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword
{
    public class SendEmailToChangePasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ChangePasswordUrl { get; set; }
    }
}