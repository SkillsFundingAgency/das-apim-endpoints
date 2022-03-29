using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Approvals.ErrorHandling
{
    public class BulkUploadApimDomainException : InvalidOperationException
    {
        public string Content { get; set; }

        /// <summary>
        /// Creates a Domain Exception with multiple domain errors
        /// </summary>
        /// <param name="errors"></param>
        public BulkUploadApimDomainException(string errors)
        {
            Content = errors;
        }
    }
}
