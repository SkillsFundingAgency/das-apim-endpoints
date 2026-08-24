using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Common.Domain.Types;
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
        var learningType = fixture.Create<LearningType>();

        var mockBodyBuilder = new Mock<IUpdateLearningRequestBodyBuilder>();
        mockBodyBuilder
            .Setup(x => x.Build(ukprn, createLearnerRequest, academicYear))
            .Setup(x => x.Build(ukprn, createLearnerRequest, learningType))
            .Returns(requestBody);

        var sut = new CreateDraftLearningApiPostRequestBuilder(mockBodyBuilder.Object);

        // Act
        var result = sut.Build(ukprn, createLearnerRequest, academicYear);
        var result = sut.Build(ukprn, createLearnerRequest, learningType);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeSameAs(requestBody);
        result.Ukprn.Should().Be(ukprn);
        result.PostUrl.Should().Be($"{ukprn}/apprenticeships");
    }
}
