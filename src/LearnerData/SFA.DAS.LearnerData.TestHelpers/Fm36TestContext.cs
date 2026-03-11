using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.TestHelpers;


/// <summary>
/// Generates test data for FM36 related tests. This should not be used for update tests as it using concrete implementations
/// from the update process to help generate the test data.
/// </summary>
public class Fm36TestContext
{
    public List<Learning> LearningInnerApiResponse { get; private set; }
    public GetFm36DataResponse EarningsInnerApiResponse { get; private set; }
    public List<UpdateLearnerRequest> SldLearnerData { get; private set; }
    public List<TestLearner> TestLearners { get; private set; }

    public Fm36TestContext()
    {
        LearningInnerApiResponse = new List<Learning>();
        EarningsInnerApiResponse = new GetFm36DataResponse { Apprenticeships = new List<Apprenticeship>() };
        SldLearnerData = new List<UpdateLearnerRequest>();
        TestLearners = new List<TestLearner>();
    }

    public void Build()
    {
        if(TestLearners.Count == 0)
        {
            TestLearners.Add(DefaultLearner.CreateNew);
        }

        foreach(var testLearner in TestLearners)
        {
            SldLearnerData.Add(testLearner.UpdateLearnerRequest);

            var learningInnerLearner = LearnerBuilder.BuildLearningInnerApiResponse(testLearner);
            LearningInnerApiResponse.Add(learningInnerLearner);

            var earningInnerLearner = LearnerBuilder.BuildEarningsInnerApiResponse(testLearner, learningInnerLearner);
            EarningsInnerApiResponse.Apprenticeships.Add(earningInnerLearner);
        }
    }
}
