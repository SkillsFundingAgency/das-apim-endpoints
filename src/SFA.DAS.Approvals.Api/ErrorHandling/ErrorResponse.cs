using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Approvals.ErrorHandling
{
    [Serializable]
    public class ErrorResponse
    {
        [JsonProperty(Required = Required.Always)]
        public List<ErrorDetail> Errors { get; }

        [JsonConstructor]
        public ErrorResponse(List<ErrorDetail> errors)
        {
            Errors = errors;
        }
    }
}
