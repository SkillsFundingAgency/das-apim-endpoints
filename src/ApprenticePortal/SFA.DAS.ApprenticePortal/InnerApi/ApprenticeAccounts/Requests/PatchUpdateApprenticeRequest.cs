using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class PatchUpdateApprenticeRequest : IPatchApiRequest<UpdateApprenticeRequest>
    {
        public string PatchUrl => $"apprentices/{Data.ApprenticeId}";
        public UpdateApprenticeRequest Data { get; set; }
    }

    public class UpdateApprenticeRequest
    {
        public Guid ApprenticeId { get; set; }
        public JsonPatchDocument<Apprentice> Patch { get; set; }
    }
}