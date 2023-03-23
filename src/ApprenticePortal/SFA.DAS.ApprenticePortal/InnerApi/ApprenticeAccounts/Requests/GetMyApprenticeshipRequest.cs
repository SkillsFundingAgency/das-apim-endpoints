using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class GetMyApprenticeshipRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetMyApprenticeshipRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"apprentices/{_id}/my-apprenticeship";
    }
}
