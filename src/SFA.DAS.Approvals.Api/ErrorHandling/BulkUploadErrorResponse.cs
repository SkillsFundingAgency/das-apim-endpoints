using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Approvals.ErrorHandling
{
    public class BulkUploadErrorResponse
    {
        public IEnumerable<BulkUploadValidationError> DomainErrors { get; set; }

        /// <summary>
        /// Creates a Domain Exception with multiple domain errors
        /// </summary>
        /// <param name="errors"></param>
        public BulkUploadErrorResponse(IEnumerable<BulkUploadValidationError> errors)
        {
            DomainErrors = errors;
        }

        public override string ToString()
        {
            return $"BulkUploadDomainException: {JsonConvert.SerializeObject(DomainErrors)}";
        }
    }
}
