using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class CostsServiceTests
    {
        [Test]
        public void With_No_Change_In_Price_Then_Costs_Are_Merged()
        {
            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var onProgrammes = command.UpdateLearnerRequest.Delivery.OnProgramme;

            var costsService = new CostsService();

            // Act
            var result = costsService.GetCosts(onProgrammes);

            //Assert
            result.Should().HaveCount(1);

            var expected = onProgrammes.First().Costs!.First();
            result.First().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void With_Change_In_Price_Then_Costs_Are_Not_Merged()
        {
            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(true);
            var onProgrammes = command.UpdateLearnerRequest.Delivery.OnProgramme;

            var costsService = new CostsService();

            // Act
            var result = costsService.GetCosts(onProgrammes);

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(onProgrammes.SelectMany(x => x.Costs));
        }
    }
}
