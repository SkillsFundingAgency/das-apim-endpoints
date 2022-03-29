//using System;
//using System.Collections.Generic;
//using Newtonsoft.Json;

//namespace SFA.DAS.Approvals.ErrorHandling
//{
//    public class DomainException : InvalidOperationException
//    {
//        public IEnumerable<DomainError> DomainErrors { get; }

//        /// <summary>
//        /// Creates a Domain Exception with a single domain error
//        /// </summary>
//        /// <param name="propertyName"></param>
//        /// <param name="errorMessage"></param>
//        public DomainException(string propertyName, string errorMessage)
//        {
//            DomainErrors = new List<DomainError>
//            {
//                new DomainError(propertyName, errorMessage)
//            };
//        }

//        /// <summary>
//        /// Creates a Domain Exception with multiple domain errors
//        /// </summary>
//        /// <param name="errors"></param>
//        public DomainException(IEnumerable<DomainError> errors)
//        {
//            DomainErrors = errors;
//        }

//        public override string ToString()
//        {
//            return $"DomainException: {JsonConvert.SerializeObject(DomainErrors)}";
//        }
//    }
//}
