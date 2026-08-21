using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface ICreateDraftLearningApiPostRequestBuilder
{
    CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear);
}

public class CreateDraftLearningApiPostRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : ICreateDraftLearningApiPostRequestBuilder
{
    public CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear)
    {
        var body = requestBodyBuilder.Build(ukprn, createLearnerRequest, academicYear);
        return new CreateDraftLearningApiPostRequest(body, ukprn);
    }
}
