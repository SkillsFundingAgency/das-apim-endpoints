using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Helpers;

namespace SFA.DAS.EmployerAccounts.UnitTests.Helpers
{
    [TestFixture]
    public class RegexHelperTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [TestCase("AB123456", true)]
        [TestCase("12345678", true)]
        [TestCase("ABCD1234", false)]
        [TestCase("XY78901Z", false)]
        public void CheckCompaniesHouseReference_ShouldReturnExpectedResult(string input, bool expectedResult)
        {
            // Act
            var result = RegexHelper.CheckCompaniesHouseReference(input);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public void CheckCompaniesHouseReference_ShouldReturnFalseForEmptyInput()
        {
            // Act
            var result = RegexHelper.CheckCompaniesHouseReference(string.Empty);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CheckCompaniesHouseReference_ShouldReturnFalseForInvalidFormat()
        {
            // Arrange
            var invalidInput = _fixture.Create<string>();

            // Act
            var result = RegexHelper.CheckCompaniesHouseReference(invalidInput);

            // Assert
            result.Should().BeFalse();
        }
    }
}
