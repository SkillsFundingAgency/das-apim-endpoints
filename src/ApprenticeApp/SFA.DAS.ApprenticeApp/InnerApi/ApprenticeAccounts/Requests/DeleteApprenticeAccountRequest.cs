using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class DeleteApprenticeAccountRequest : IDeleteApiRequest
    {
        private readonly Guid _apprenticeId;

        public DeleteApprenticeAccountRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }

        public string DeleteUrl => $"apprentices/{_apprenticeId}";
    }
}
