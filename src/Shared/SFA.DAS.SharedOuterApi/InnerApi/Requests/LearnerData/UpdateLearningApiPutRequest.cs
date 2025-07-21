using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class UpdateLearningApiPutRequest : IPutApiRequest<UpdateLearningRequestBody>
{
    public string PutUrl { get; }

    public UpdateLearningRequestBody Data { get; set; }

    public UpdateLearningApiPutRequest(Guid learningKey, UpdateLearningRequestBody data)
    {
        PutUrl = learningKey.ToString();
        Data = data;
    }
}

public class UpdateLearningRequestBody
{
    public LearningUpdateDetails Learner { get; set; }
}

public class LearningUpdateDetails
{
    public DateTime? CompletionDate { get; set; }
}
