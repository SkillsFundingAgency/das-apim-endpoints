using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface ICreateDraftLearningApiPostRequestBuilder
{
    CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear, LearningType learningType);
}

public class CreateDraftLearningApiPostRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : ICreateDraftLearningApiPostRequestBuilder
{
    public CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear, LearningType learningType)
    {
        var body = requestBodyBuilder.Build(ukprn, createLearnerRequest, academicYear, learningType);
        return new CreateDraftLearningApiPostRequest(body, ukprn);
    }
}
