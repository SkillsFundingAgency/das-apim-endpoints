using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetApprenticePreferencesRequest : IGetApiRequest
    {
        public Guid ApprenticeId { get; set; }

        public GetApprenticePreferencesRequest(Guid id)
        {
            ApprenticeId = id;
        }

        public string GetUrl => $"apprenticepreferences/{ApprenticeId}";
    }
}
