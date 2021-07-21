using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutEmployerVendorIdForLegalEntityRequest : IPutApiRequest<PutEmployerVendorIdForLegalEntityRequestData>
    {
        public string PutUrl => $"legalentities/{Data.HashedLegalEntityId}/employervendorid";
        public PutEmployerVendorIdForLegalEntityRequestData Data { get; set; }
    }

    public class PutEmployerVendorIdForLegalEntityRequestData
    {
        public string HashedLegalEntityId { get; set; }
        public string EmployerVendorId { get; set; }
    }
}