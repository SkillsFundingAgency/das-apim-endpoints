using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface ICreateDraftLearningApiPostRequestBuilder
{
    CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear);
    CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest,
        LearningType learningType);
}

public class CreateDraftLearningApiPostRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : ICreateDraftLearningApiPostRequestBuilder
{
    public CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear)
    public CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest,
        LearningType learningType)
    {
        var body = requestBodyBuilder.Build(ukprn, createLearnerRequest, academicYear);
        var body = requestBodyBuilder.Build(ukprn, createLearnerRequest, learningType);
        return new CreateDraftLearningApiPostRequest(body, ukprn);
    }
}
