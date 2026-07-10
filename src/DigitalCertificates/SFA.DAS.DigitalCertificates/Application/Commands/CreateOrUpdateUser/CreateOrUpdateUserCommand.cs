using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserResult>
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
