using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models
{
    public record RegistrationDetails
    {
        private string _emailAddress;

        public string FirstName { get; set; }

        public string MiddleNames { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Address Address { get; set; } = new Address();

        public string EmailAddress
        {
            get => !string.IsNullOrWhiteSpace(_emailAddress) ? _emailAddress.ToLower() : _emailAddress;
            set => _emailAddress = value;
        }

        public string PhoneNumber { get; set; }

        public string AcceptedTermsAndConditionsVersion { get; set; }
    }
}
