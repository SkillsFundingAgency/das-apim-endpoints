using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeAccounts
{
    public class GetMyApprenticeshipRequest : IGetApiRequest
    {
        public Guid Id { get; }

        public GetMyApprenticeshipRequest(Guid id)
        {
            Id = id;
        }

        public string GetUrl => $"apprentices/{Id}/MyApprenticeship";
    }
}
