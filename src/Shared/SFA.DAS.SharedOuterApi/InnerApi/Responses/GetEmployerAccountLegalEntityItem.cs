using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetEmployerAccountLegalEntityItem
    {
        [JsonPropertyName("Name")]
        public string AccountLegalEntityName { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string AccountPublicHashedId { get ; set ; }
        public string AccountName { get ; set ; }
        public List<Agreement> Agreements { get; set; }
        public long AccountLegalEntityId { get; set; }
    }
    
    public class Agreement
    {
        public long Id { get; set; }
        public EmployerAgreementStatus Status { get; set; }
        public int AgreementType { get; set; }
        public int TemplateVersionNumber { get; set; }
        public long? SignedById { get; set; }
        public string? SignedByName { get; set; }
        public DateTime? SignedDate { get; set; }
        public string? SignedByEmail { get; set; }
    }
    
    public enum EmployerAgreementStatus : byte
    {
        Pending = 1,
        Signed = 2,
        Expired = 3,
        Superseded = 4,
        Removed = 5
    }
}