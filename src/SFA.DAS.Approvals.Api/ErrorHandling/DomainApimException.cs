using System;

namespace SFA.DAS.Approvals.ErrorHandling
{
    public class DomainApimException : InvalidOperationException
    {
        public string Content { get; set;  }

        /// <summary>
        /// Creates a Domain Exception with multiple domain errors
        /// </summary>
        /// <param name="errors"></param>
        public DomainApimException(string errors)
        {
            Content = errors;
        }
    
    }
}
