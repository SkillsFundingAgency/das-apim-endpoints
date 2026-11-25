using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class CreateUserActionResponse
    {
        public string ActionCode { get; set; }

        public static implicit operator CreateUserActionResponse(CreateUserActionResult source)
        {
            if (source == null) return null;

            return new CreateUserActionResponse
            {
                ActionCode = source.ActionCode
            };
        }
    }
}
