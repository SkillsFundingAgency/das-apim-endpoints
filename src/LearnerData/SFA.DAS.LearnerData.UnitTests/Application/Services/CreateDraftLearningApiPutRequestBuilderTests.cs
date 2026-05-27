using AutoFixture;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class CreateDraftLearningApiPutRequestBuilderTests
{
    [Test]
    public void Build_DelegatesToRequestBodyBuilder_And_ReturnsCreateDraftLearningApiPutRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var ukprn = fixture.Create<long>();
        var updateLearnerRequest = fixture.Create<UpdateLearnerRequest>();
        var requestBody = fixture.Create<UpdateLearningRequestBody>();

        var mockBodyBuilder = new Mock<IUpdateLearningRequestBodyBuilder>();
        mockBodyBuilder
            .Setup(x => x.Build(ukprn, updateLearnerRequest))
            .Returns(requestBody);

        var sut = new CreateDraftLearningApiPutRequestBuilder(mockBodyBuilder.Object);

        // Act
        var result = sut.Build(ukprn, updateLearnerRequest);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeSameAs(requestBody);
        result.Ukprn.Should().Be(ukprn);
        result.PutUrl.Should().Be($"{ukprn}/apprenticeship{updateLearnerRequest.Learner.Uln}");
    }
}