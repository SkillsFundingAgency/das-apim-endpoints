using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class GetCurrentApprenticeshipRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetCurrentApprenticeshipRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"apprentices/{_id}/apprenticeships/current";
    }
}
