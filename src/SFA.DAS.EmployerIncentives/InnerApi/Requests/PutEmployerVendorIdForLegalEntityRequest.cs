using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutEmployerVendorIdForLegalEntityRequest : IPutApiRequest
    {
        private readonly string _hashedLegalEntityId;

        public PutEmployerVendorIdForLegalEntityRequest(string hashedLegalEntityId)
        {
            _hashedLegalEntityId = hashedLegalEntityId;
        }

        public string PutUrl => $"legalentities/{_hashedLegalEntityId}/employervendorid";
        public object Data { get; set; }
    }

    public class PutEmployerVendorIdForLegalEntityRequestData
    {
        public string EmployerVendorId { get; set; }
    }
}