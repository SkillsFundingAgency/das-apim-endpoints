using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

public class UpdateLearnerApiPutResponse : List<LearningUpdateChanges>
{

}

public enum LearningUpdateChanges
{
    CompletionDate = 0,
    MathsAndEnglish = 1,
    LearningSupport = 2
}