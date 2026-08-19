using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class CreateDraftLearningApiPostRequestBuilderTests
{
    [Test]
    public void Build_DelegatesToRequestBodyBuilder_And_ReturnsCreateDraftLearningApiPostRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var ukprn = fixture.Create<long>();
        var academicYear = fixture.Create<int>();
        var createLearnerRequest = fixture.Create<CreateLearnerRequest>();
        var requestBody = fixture.Create<UpdateLearningRequestBody>();

        var mockBodyBuilder = new Mock<IUpdateLearningRequestBodyBuilder>();
        mockBodyBuilder
            .Setup(x => x.Build(ukprn, createLearnerRequest, academicYear))
            .Returns(requestBody);

        var sut = new CreateDraftLearningApiPostRequestBuilder(mockBodyBuilder.Object);

        // Act
        var result = sut.Build(ukprn, createLearnerRequest, academicYear);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeSameAs(requestBody);
        result.Ukprn.Should().Be(ukprn);
        result.PostUrl.Should().Be($"{ukprn}/apprenticeships");
    }
}
