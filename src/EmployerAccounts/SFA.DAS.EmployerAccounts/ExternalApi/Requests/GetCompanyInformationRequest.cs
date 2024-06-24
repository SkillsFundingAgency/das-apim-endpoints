using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Requests
{
    public class GetCompanyInformationRequest : IGetApiRequest
    {
        public string Id { get; }
        public GetCompanyInformationRequest(string id)
        {
            Id = !string.IsNullOrEmpty(id) ? id.ToUpper() : "";
        }

        public string GetUrl => $"company/{Id}";
    }
}
