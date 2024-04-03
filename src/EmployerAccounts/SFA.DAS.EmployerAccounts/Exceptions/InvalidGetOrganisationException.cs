namespace SFA.DAS.EmployerAccounts.Exceptions
{
    public class InvalidGetOrganisationException : ReferenceDataException
    {
        public InvalidGetOrganisationException()
        {
        }

        public InvalidGetOrganisationException(string message)
            : base(message)
        {
        }
    }
}
