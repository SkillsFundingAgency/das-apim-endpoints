using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Extensions;

namespace SFA.DAS.ApprenticeApp.UnitTests.Extensions
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void GetApprenticeshipType_ReturnsCorrectInt_ForValidEnumString()
        {
            // Arrange
            string input = "Apprenticeship";
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual((int)ApprenticeshipType.Apprenticeship, result);
        }

        [Test]
        public void GetApprenticeshipType_ReturnsCorrectInt_ForCaseInsensitiveInput()
        {
            // Arrange
            string input = "fOuNdAtIoNaPpReNtIcEsHiP";
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual((int)ApprenticeshipType.FoundationApprenticeship, result);
        }

        [Test]
        public void GetApprenticeshipType_ReturnsDefault_ForInvalidString()
        {
            // Arrange
            string input = "InvalidValue";
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual(null, result);
        }

        [Test]
        public void GetApprenticeshipType_ReturnsDefault_ForEmptyString()
        {
            // Arrange
            string input = "";
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual(null, result);
        }

        [Test]
        public void GetApprenticeshipType_ReturnsDefault_ForNullString()
        {
            // Arrange
            string? input = null;
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual(null, result);
        }

        [Test]
        public void GetApprenticeshipType_ReturnsCustomDefault_WhenSpecified()
        {
            // Arrange
            string input = "InvalidValue";
            int customDefault = 99;
            
            // Act
            int? result = input.GetApprenticeshipType(customDefault);
            
            // Assert
            ClassicAssert.AreEqual(customDefault, result);
        }

        [Test]
        public void GetApprenticeshipType_ReturnsCorrectValue_ForWhitespaceString()
        {
            // Arrange
            string input = "   ";
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual(null, result);
        }
        
        [Test]
        public void GetApprenticeshipType_ReturnsCorrectValue_ForPartialMatch()
        {
            // Arrange
            string input = "Foundation";
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual(null, result); // Should return default since it's not exact match
        }

        [Test]
        public void GetApprenticeshipType_ReturnsCorrectValue_ForDifferentCulture()
        {
            // Arrange
            string input = "Apprenticeship".ToUpperInvariant(); // Test different casing
            
            // Act
            int? result = input.GetApprenticeshipType();
            
            // Assert
            ClassicAssert.AreEqual((int)ApprenticeshipType.Apprenticeship, result);
        }


    }
}