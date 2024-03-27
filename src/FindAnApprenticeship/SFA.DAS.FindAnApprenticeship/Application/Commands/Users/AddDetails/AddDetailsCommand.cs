using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.AddDetails
{
    public class AddDetailsCommand : IRequest<Unit>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GovUkIdentifier { get; set; }
        public string Email { get; set; }
    }
}
