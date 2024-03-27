using System;

namespace SFA.DAS.EmployerAccounts.Exceptions
{
    public class ReferenceDataException : Exception
    {
        public ReferenceDataException()
        {
        }

        public ReferenceDataException(string message)
            : base(message)
        {
        }
    }
}