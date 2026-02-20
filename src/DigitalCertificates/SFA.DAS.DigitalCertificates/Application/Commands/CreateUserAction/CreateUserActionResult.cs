using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionResult
    {
        public string ActionCode { get; set; }

        public static implicit operator CreateUserActionResult(PostCreateUserActionResponse response)
        {
            return new CreateUserActionResult
            {
                ActionCode = response?.ActionCode
            };
        }
    }
}
