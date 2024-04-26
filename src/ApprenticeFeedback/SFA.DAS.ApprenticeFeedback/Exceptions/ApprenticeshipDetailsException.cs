using System;

namespace SFA.DAS.ApprenticeFeedback.Exceptions
{
    public class ApprenticeshipDetailsException : Exception
    {
        public ApprenticeshipDetailsException()
        {
        }

        public ApprenticeshipDetailsException(string message)
            : base(message)
        {
        }

        public ApprenticeshipDetailsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
