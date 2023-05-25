using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class PatchApprenticeRequest : IPatchApiRequest<JsonPatchDocument<Apprentice>>
    {
        private readonly Guid _apprenticeId;

        public PatchApprenticeRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }
        public string PatchUrl => $"apprentices/{_apprenticeId}";
        public JsonPatchDocument<Apprentice> Data { get; set; }
    }
}