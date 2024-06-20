using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Requests
{
    public class GetCompanyInformationRequest : IGetApiRequest
    {
        public string Id { get; }
        public int MaximumResults { get; set; }

        public GetCompanyInformationRequest(string id)
        {
            Id = id;
        }

        public string GetUrl => $"company/{Id}";
    }
}
