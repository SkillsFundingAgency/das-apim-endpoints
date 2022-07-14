using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class GetApprenticeRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetApprenticeRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"apprentices/{_id}";
    }
}
