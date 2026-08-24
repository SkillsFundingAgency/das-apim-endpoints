using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class PatchMyApprenticeshipRequest : IPatchApiRequest<object>
    {
        private readonly Guid _apprenticeId;

        public PatchMyApprenticeshipRequest(Guid apprenticeId, object data)
        {   
            _apprenticeId = apprenticeId;
            Data = data;
        }

        public string PatchUrl => $"apprentice/{_apprenticeId}/MyApprenticeship";
        public object Data { get; set; }
    }
}
