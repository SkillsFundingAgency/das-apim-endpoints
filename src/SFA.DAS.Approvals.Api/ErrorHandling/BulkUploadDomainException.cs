//using System;
//using System.Collections.Generic;
//using Newtonsoft.Json;

//namespace SFA.DAS.Approvals.ErrorHandling
//{
//    public class BulkUploadDomainException : InvalidOperationException
//    {
//        public IEnumerable<BulkUploadValidationError> DomainErrors { get; }

//        /// <summary>
//        /// Creates a Domain Exception with multiple domain errors
//        /// </summary>
//        /// <param name="errors"></param>
//        public BulkUploadDomainException(IEnumerable<BulkUploadValidationError> errors)
//        {
//            DomainErrors = errors;
//        }

//        public override string ToString()
//        {
//            return $"BulkUploadDomainException: {JsonConvert.SerializeObject(DomainErrors)}";
//        }
//    }
//}
