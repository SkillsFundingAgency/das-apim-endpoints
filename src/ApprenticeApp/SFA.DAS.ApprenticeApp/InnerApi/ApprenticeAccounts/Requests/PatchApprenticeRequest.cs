using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class PatchApprenticeRequest : IPatchApiRequest<object>
    {
        private readonly Guid _apprenticeId;

        public PatchApprenticeRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }
        public string PatchUrl => $"apprentices/{_apprenticeId}";
        public object Data { get; set; }

    }
}