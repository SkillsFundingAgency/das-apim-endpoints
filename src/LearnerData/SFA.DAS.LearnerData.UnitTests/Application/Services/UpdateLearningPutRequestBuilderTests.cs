using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LearnerData.Services;
using FluentAssertions;
using Moq;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class UpdateLearningPutRequestBuilderTests
    {

        [Test]
        public void Build_Sets_WithdrawalDate_From_LatestOnProgramme()
        {
            var fixture = new Fixture();

            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var withdrawalDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().WithdrawalDate = withdrawalDate;

            var sut = new UpdateLearningPutRequestBuilder(
                Mock.Of<ILearningSupportService>(),
                Mock.Of<IBreaksInLearningService>(),
                Mock.Of<ICostsService>());

            // Act
            var actualRequest = sut.Build(command);

            // Assert
            actualRequest.Data.Delivery.WithdrawalDate.Should().Be(withdrawalDate);
        }

        [Test]
        public void Build_Sets_CompletionDate_From_LatestOnProgramme()
        {
            var fixture = new Fixture();

            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var completionDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().CompletionDate = completionDate;

            var sut = new UpdateLearningPutRequestBuilder(
                Mock.Of<ILearningSupportService>(),
                Mock.Of<IBreaksInLearningService>(),
                Mock.Of<ICostsService>());

            // Act
            var actualRequest = sut.Build(command);

            // Assert
            actualRequest.Data.Learner.CompletionDate.Should().Be(completionDate);
        }

        [Test]
        public void Build_Sets_ExpectedEndDate_From_LatestOnProgramme()
        {
            var fixture = new Fixture();

            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var expectedEndDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().ExpectedEndDate = expectedEndDate;

            var sut = new UpdateLearningPutRequestBuilder(
                Mock.Of<ILearningSupportService>(),
                Mock.Of<IBreaksInLearningService>(),
                Mock.Of<ICostsService>());

            // Act
            var actualRequest = sut.Build(command);

            // Assert
            actualRequest.Data.OnProgramme.ExpectedEndDate.Should().Be(expectedEndDate);
        }

        [Test]
        public void Build_Sets_PauseDate_From_LatestOnProgramme()
        {
            var fixture = new Fixture();

            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var pauseDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().PauseDate = pauseDate;

            var sut = new UpdateLearningPutRequestBuilder(
                Mock.Of<ILearningSupportService>(),
                Mock.Of<IBreaksInLearningService>(),
                Mock.Of<ICostsService>());

            // Act
            var actualRequest = sut.Build(command);

            // Assert
            actualRequest.Data.OnProgramme.PauseDate.Should().Be(pauseDate);
        }
    }
}
