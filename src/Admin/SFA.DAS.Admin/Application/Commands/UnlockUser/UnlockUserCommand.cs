using MediatR;
using System;

namespace SFA.DAS.Admin.Application.Commands.UnlockUser
{
    public class UnlockUserCommand : IRequest<Unit?>
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public long UserActionId { get; set; }
    }
}
