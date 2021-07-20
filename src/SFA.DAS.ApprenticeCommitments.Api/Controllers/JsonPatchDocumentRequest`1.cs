using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    public class JsonPatchDocumentRequest<T> : IPatchApiRequest<JsonPatchDocument<T>> where T : class
    {
        public string PatchUrl { get; set; }

        public JsonPatchDocument<T> Data { get; set; }
    }
}