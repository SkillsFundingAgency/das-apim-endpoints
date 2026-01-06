using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class UpdateLearningSupportApiPutRequest(Guid learningKey, UpdateLearningSupportRequest data) : IPutApiRequest<UpdateLearningSupportRequest>
{
    public string PutUrl { get; } = $"learning/{learningKey}/learning-support";

    public UpdateLearningSupportRequest Data { get; set; } = data;
}

public class UpdateLearningSupportRequest
{
    public List<LearningSupportItem> LearningSupport { get; set; } = [];
}

public class LearningSupportItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}