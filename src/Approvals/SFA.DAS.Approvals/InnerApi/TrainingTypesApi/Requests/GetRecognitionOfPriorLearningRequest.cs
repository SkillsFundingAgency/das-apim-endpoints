using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Requests
{
    public class GetRecognitionOfPriorLearningRequest(string trainingTypeShortCode) : IGetApiRequest
    {
        public string GetUrl => $"api/trainingtypes/{trainingTypeShortCode}/features/rpl";
    }
} 