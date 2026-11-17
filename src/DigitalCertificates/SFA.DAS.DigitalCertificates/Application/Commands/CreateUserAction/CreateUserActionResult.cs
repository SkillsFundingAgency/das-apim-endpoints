using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionResult
    {
        public string ActionCode { get; set; }

        public static implicit operator CreateUserActionResult(CreateUserActionResponse response)
        {
            return new CreateUserActionResult
            {
                ActionCode = response?.ActionCode
            };
        }
    }
}
