using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Services;
using System;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class UpdateLearningPutRequestBuilderTests
{
    [Test]
    public void Build_DelegatesToRequestBodyBuilder_And_ReturnsUpdateLearningApiPutRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var ukprn = fixture.Create<long>();
        var learningKey = fixture.Create<Guid>();
        var updateLearnerRequest = fixture.Create<UpdateLearnerRequest>();
        var requestBody = fixture.Create<UpdateLearningRequestBody>();

        var mockBodyBuilder = new Mock<IUpdateLearningRequestBodyBuilder>();
        mockBodyBuilder
            .Setup(x => x.Build(ukprn, updateLearnerRequest))
            .Returns(requestBody);

        var sut = new UpdateLearningPutRequestBuilder(mockBodyBuilder.Object);

        // Act
        var result = sut.Build(ukprn, updateLearnerRequest, learningKey);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeSameAs(requestBody);
        result.PutUrl.Should().Be(learningKey.ToString());
    }
}