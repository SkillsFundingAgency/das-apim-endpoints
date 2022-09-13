using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.VacanciesManage.Api.Models
{

    public class CreateVacancyExampleForbiddenResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.3</example>
        [JsonPropertyName("type")]
        public Uri Type { get; set; }

        /// <summary>
        /// Title of the error response
        /// </summary>
        /// <example>Forbidden</example>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The http status code of the error
        /// </summary>
        /// <example>403</example>
        [JsonPropertyName("status")]
        public long Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>|3f20b24dba787449b33e0d53e3d3ef9d.d6137015_</example>
        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }
    
    public class CreateVacancyExampleBadRequestResponse 
    {
        /// <summary>
        /// List of errors returned from the response
        /// </summary>
        [JsonPropertyName("errors")]
        public Errors Errors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.1</example>
        [JsonPropertyName("type")]
        public Uri Type { get; set; }

        /// <summary>
        /// Title of the error response
        /// </summary>
        /// <example>One or more validation errors occurred.</example>
        [JsonPropertyName("title")]
        public string Title { get; set; }


        /// <summary>
        /// The http status code of the error
        /// </summary>
        /// <example>400</example>
        [JsonPropertyName("status")]
        public long Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>|3f20b24dba787449b33e0d53e3d3ef9d.d6137015_</example>
        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }
    
    public class Errors
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>["Required property 'qualifications' not found in JSON. Path '', line 42, position 1."]</example>
        [JsonPropertyName("qualifications")]
        public List<string> Qualifications { get; set; }
    }
}