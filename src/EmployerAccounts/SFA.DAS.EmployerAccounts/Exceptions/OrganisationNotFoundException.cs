using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Exceptions
{
    public class OrganisationNotFoundException : ReferenceDataException
    {
        public OrganisationType OrganisationType { get; }

        public string Identifier { get; }

        public OrganisationNotFoundException(string message)
            : base(message)
        {
        }

        public OrganisationNotFoundException(OrganisationType organisationType, string identifier)
            : base($"Did not find an organisation type {organisationType} with identifier {identifier}")
        {
            OrganisationType = organisationType;
            Identifier = identifier;
        }
    }
}
