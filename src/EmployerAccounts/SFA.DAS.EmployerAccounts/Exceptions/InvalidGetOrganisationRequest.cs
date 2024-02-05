namespace SFA.DAS.EmployerAccounts.Exceptions
{
    public class InvalidGetOrganisationRequest : ReferenceDataException
    {
        public InvalidGetOrganisationRequest()
        {
        }

        public InvalidGetOrganisationRequest(string message)
            : base(message)
        {
        }
    }
}
