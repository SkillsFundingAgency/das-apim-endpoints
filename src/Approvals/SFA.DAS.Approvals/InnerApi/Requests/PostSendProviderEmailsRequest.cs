using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostSendProviderEmailsRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }
        public string PostUrl => $"api/email/{ProviderId}/send";

        public PostSendProviderEmailsRequest(long providerId, ProviderEmailRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }

    public class ProviderEmailRequest
    {
        public string TemplateId { get; set; }

        public Dictionary<string, string> Tokens { get; set; }

        public List<string> ExplicitEmailAddresses { get; set; }
    }
}
