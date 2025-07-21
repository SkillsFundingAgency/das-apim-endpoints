using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class SaveCompletionApiPutRequest : IPatchApiRequest<SaveCompletionRequest>
{
    public string PatchUrl { get; }

    public SaveCompletionRequest Data { get; set; }

    public SaveCompletionApiPutRequest(Guid learningKey, SaveCompletionRequest data)
    {
        PatchUrl = $"apprenticeship/{learningKey.ToString()}/completion";
        Data = data;
    }
}

public class SaveCompletionRequest
{
    public DateTime? CompletionDate { get; set; }
}
