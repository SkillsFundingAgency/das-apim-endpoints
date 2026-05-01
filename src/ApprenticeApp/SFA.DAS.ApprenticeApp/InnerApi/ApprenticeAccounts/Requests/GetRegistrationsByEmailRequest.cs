using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetRegistrationsByEmailRequest : IGetApiRequest
    {
        private readonly string _email;

        public GetRegistrationsByEmailRequest(string email)
        {
            _email = email;
        }

        public string GetUrl => $"registrations/email?email={_email}";
    }
}
