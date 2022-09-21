using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class ApprenticeNotFoundException : Exception
    {
        public ApprenticeNotFoundException(string message) : base(message) { }
    }
}
