using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetAllowEmployerAddRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Constructed_With_LearningType()
    {
        var learningType = "ApprenticeshipUnit";
        var actual = new GetAllowEmployerAddRequest(learningType);

        actual.GetUrl.Should().Contain("api/coursetypes/features/allowEmployerAdd");
        actual.GetUrl.Should().Contain("learningType=ApprenticeshipUnit");
        actual.LearningType.Should().Be(learningType);
    }

    [Test]
    public void Then_The_Url_Encodes_LearningType()
    {
        var learningType = "FoundationApprenticeship";
        var actual = new GetAllowEmployerAddRequest(learningType);

        actual.GetUrl.Should().Contain("learningType=FoundationApprenticeship");
    }
}
