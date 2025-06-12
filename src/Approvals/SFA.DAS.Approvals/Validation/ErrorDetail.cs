using System;
using Newtonsoft.Json;

namespace SFA.DAS.Approvals.Validation
{
    [Serializable]
    public class ErrorDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        [JsonProperty(Required = Required.Always)]
        public string Message { get; }

        [JsonConstructor]
        public ErrorDetail(string field, string message)
        {
            Field = string.IsNullOrWhiteSpace(field) ? null : field;
            Message = message;
        }
    }
}
