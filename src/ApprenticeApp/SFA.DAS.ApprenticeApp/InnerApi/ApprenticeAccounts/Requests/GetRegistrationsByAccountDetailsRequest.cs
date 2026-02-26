using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetRegistrationsByAccountDetailsRequest : IGetApiRequest
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _dateOfBirth;

        public GetRegistrationsByAccountDetailsRequest(string firstName, string lastName, string dateOfBirth)
        {
            _firstName = firstName;
            _lastName = lastName;
            _dateOfBirth = dateOfBirth;
        }

        public string GetUrl => $"registrations?firstName={_firstName}&lastName={_lastName}&dateOfBirth={_dateOfBirth}";
    }
}
