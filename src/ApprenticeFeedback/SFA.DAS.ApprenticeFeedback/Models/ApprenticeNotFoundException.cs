using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class ApprenticeNotFoundException : Exception
    {
        public ApprenticeNotFoundException(string message) : base(message) { }
    }
}
