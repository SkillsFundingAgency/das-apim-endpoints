using System;
using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public Guid Id { get ; set ; }
        public string Password { get; set; }
    }
}