using NUnit.Framework;
using System;
using NUnit.Framework.Legacy;
using SFA.DAS.ApprenticeCommitments.Types;

namespace SFA.DAS.ApprenticeCommitments.Types.UnitTests.Types
{
    [TestFixture]
    public class ApprenticeshipTypeTests
    {
        [Test]
        public void Enum_Has_Correct_Underlying_Type()
        {
            // Arrange & Act
            var underlyingType = Enum.GetUnderlyingType(typeof(ApprenticeshipType));

            // Assert
            ClassicAssert.AreEqual(typeof(int), underlyingType);
        }

        [Test]
        public void Enum_Has_Expected_Members()
        {
            // Arrange
            var values = Enum.GetValues(typeof(ApprenticeshipType));

            // Assert
            ClassicAssert.AreEqual(2, values.Length, "Should have exactly 2 defined values");
            ClassicAssert.That(values, Contains.Item(ApprenticeshipType.Apprenticeship));
            ClassicAssert.That(values, Contains.Item(ApprenticeshipType.FoundationApprenticeship));
        }

        [TestCase(ApprenticeshipType.Apprenticeship, 0)]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, 1)]
        public void Enum_Members_Have_Correct_Integer_Values(
            ApprenticeshipType type, int expectedValue)
        {
            // Act & Assert
            ClassicAssert.AreEqual(expectedValue, (int)type);
        }

        [TestCase(0, ApprenticeshipType.Apprenticeship)]
        [TestCase(1, ApprenticeshipType.FoundationApprenticeship)]
        public void Can_Convert_Integer_To_Enum(int value, ApprenticeshipType expected)
        {
            // Act
            var result = (ApprenticeshipType)value;

            // Assert
            ClassicAssert.AreEqual(expected, result);
        }

        [TestCase("Apprenticeship", ApprenticeshipType.Apprenticeship)]
        [TestCase("FoundationApprenticeship", ApprenticeshipType.FoundationApprenticeship)]
        [TestCase("apprenticeship", ApprenticeshipType.Apprenticeship)] // Case-insensitive
        [TestCase("FOUNDATIONAPPRENTICESHIP", ApprenticeshipType.FoundationApprenticeship)]
        public void Can_Parse_Valid_Strings(string value, ApprenticeshipType expected)
        {
            // Act
            var success = Enum.TryParse(value, true, out ApprenticeshipType result);

            // Assert
            ClassicAssert.IsTrue(success);
            ClassicAssert.AreEqual(expected, result);
        }

        [TestCase("InvalidValue")]
        [TestCase("")]
        [TestCase(null)]
        public void Parse_Invalid_String_Returns_False(string value)
        {
            // Act
            var success = Enum.TryParse(value, true, out ApprenticeshipType _);

            // Assert
            ClassicAssert.IsFalse(success);
        }

        [TestCase(2)]
        [TestCase(-1)]
        [TestCase(99)]
        public void Invalid_Integer_Value_Not_Defined(int value)
        {
            // Act & Assert
            ClassicAssert.IsFalse(Enum.IsDefined(typeof(ApprenticeshipType), value));
        }

        [Test]
        public void ToString_Returns_Expected_Names()
        {
            // Arrange
            var apprenticeship = ApprenticeshipType.Apprenticeship;
            var foundation = ApprenticeshipType.FoundationApprenticeship;

            // Act & Assert
            ClassicAssert.AreEqual("Apprenticeship", apprenticeship.ToString());
            ClassicAssert.AreEqual("FoundationApprenticeship", foundation.ToString());
        }

        [Test]
        public void GetNames_Returns_Correct_Values()
        {
            // Act
            var names = Enum.GetNames(typeof(ApprenticeshipType));

            // Assert
            ClassicAssert.AreEqual(2, names.Length);
            ClassicAssert.Contains("Apprenticeship", names);
            ClassicAssert.Contains("FoundationApprenticeship", names);
        }
    }
}