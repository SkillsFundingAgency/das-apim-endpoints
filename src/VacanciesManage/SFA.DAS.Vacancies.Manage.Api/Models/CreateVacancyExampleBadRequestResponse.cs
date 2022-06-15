using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.Manage.Api.Models
{

    public class CreateVacancyExampleForbiddenResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.3</example>
        [JsonProperty("type")]
        public Uri Type { get; set; }

        /// <summary>
        /// Title of the error response
        /// </summary>
        /// <example>Forbidden</example>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The http status code of the error
        /// </summary>
        /// <example>403</example>
        [JsonProperty("status")]
        public long Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>|3f20b24dba787449b33e0d53e3d3ef9d.d6137015_</example>
        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }
    
    public class CreateVacancyExampleBadRequestResponse 
    {
        /// <summary>
        /// List of errors returned from the response
        /// </summary>
        [JsonProperty("errors")]
        public Errors Errors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.1</example>
        [JsonProperty("type")]
        public Uri Type { get; set; }

        /// <summary>
        /// Title of the error response
        /// </summary>
        /// <example>One or more validation errors occurred.</example>
        [JsonProperty("title")]
        public string Title { get; set; }


        /// <summary>
        /// The http status code of the error
        /// </summary>
        /// <example>400</example>
        [JsonProperty("status")]
        public long Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>|3f20b24dba787449b33e0d53e3d3ef9d.d6137015_</example>
        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }
    
    public class Errors
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>["Required property 'qualifications' not found in JSON. Path '', line 42, position 1."]</example>
        [JsonProperty("qualifications")]
        public List<string> Qualifications { get; set; }
    }
}