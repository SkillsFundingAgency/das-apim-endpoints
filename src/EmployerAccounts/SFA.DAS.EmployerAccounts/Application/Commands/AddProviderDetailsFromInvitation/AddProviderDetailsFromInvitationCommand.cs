using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Commands.AddProviderDetailsFromInvitation
{
    public class AddProviderDetailsFromInvitationCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string CorrelationId { get; set; }

        public long AccountId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
