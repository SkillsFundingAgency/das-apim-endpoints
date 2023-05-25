using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class PatchApprenticeRequest : IPatchApiRequest<PatchApprentice>
    {
        public string PatchUrl => $"apprentices/{Data.ApprenticeId}";
        public PatchApprentice Data { get; set; }
    }

    public class PatchApprentice
    {
        public Guid ApprenticeId { get; set; }
        public JsonPatchDocument<Apprentice> Patch { get; set; }
    }
}