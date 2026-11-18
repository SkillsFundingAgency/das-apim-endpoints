#nullable enable

using MediatR;

namespace SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserResult>
    {
        public required string DfeSignInUserId { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}