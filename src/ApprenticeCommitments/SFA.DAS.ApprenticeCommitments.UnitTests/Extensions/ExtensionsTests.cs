using System;
using System.Globalization;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Types;
using NUnit.Framework.Legacy;
using SFA.DAS.ApprenticeCommitments.Extensions;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.Extensions
{
    [TestFixture]
    public class ExtensionTests
    {
        private const string ApprenticeshipTypeName = "Apprenticeship";
        private const string FoundationApprenticeshipTypeName = "FoundationApprenticeship";
        private const string MixedCaseTypeName = "aPpReNtIcEsHiP";
        private const string InvalidTypeName = "InvalidValue";

        [TestFixture]
        public class ToIsoDateTimeTests
        {
            [Test]
            public void Should_Format_DateTime_Correctly()
            {
                // Arrange
                var date = new DateTime(2023, 10, 15, 14, 30, 45, DateTimeKind.Utc);
                
                // Act
                var result = date.ToIsoDateTime();
                
                // Assert
                ClassicAssert.AreEqual("2023-10-15T14:30:45Z", result);
            }

            [Test]
            public void Should_Handle_MinValue()
            {
                // Arrange
                var date = DateTime.MinValue;
                
                // Act
                var result = date.ToIsoDateTime();
                
                // Assert
                ClassicAssert.AreEqual("0001-01-01T00:00:00Z", result);
            }

            [Test]
            public void Should_Handle_MaxValue()
            {
                // Arrange
                var date = DateTime.MaxValue;
                
                // Act
                var result = date.ToIsoDateTime();
                
                // Assert
                ClassicAssert.AreEqual("9999-12-31T23:59:59Z", result);
            }

            [Test]
            public void Should_Use_Invariant_Culture()
            {
                // Arrange
                var originalCulture = CultureInfo.CurrentCulture;
                CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
                var date = new DateTime(2023, 10, 15, 14, 30, 45, DateTimeKind.Utc);
                
                try
                {
                    // Act
                    var result = date.ToIsoDateTime();
                    
                    // Assert
                    ClassicAssert.AreEqual("2023-10-15T14:30:45Z", result);
                }
                finally
                {
                    // Reset culture
                    CultureInfo.CurrentCulture = originalCulture;
                }
            }
        }

        [TestFixture]
        public class GetApprenticeshipTypeFromStringTests
        {
            [Test]
            public void Should_Return_Apprenticeship_For_Valid_String()
            {
                // Act
                var result = ApprenticeshipTypeName.GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual((int)ApprenticeshipType.Apprenticeship, result);
            }

            [Test]
            public void Should_Return_Foundation_For_Valid_String()
            {
                // Act
                var result = FoundationApprenticeshipTypeName.GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual((int)ApprenticeshipType.FoundationApprenticeship, result);
            }

            [Test]
            public void Should_Be_Case_Insensitive()
            {
                // Act
                var result = MixedCaseTypeName.GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual((int)ApprenticeshipType.Apprenticeship, result);
            }

            [Test]
            public void Should_Return_Default_For_Invalid_String()
            {
                // Act
                var result = InvalidTypeName.GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual(0, result);
            }

            [Test]
            public void Should_Return_Default_For_Null_Input()
            {
                // Arrange
                string nullString = null;
                
                // Act
                var result = nullString.GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual(0, result);
            }

            [Test]
            public void Should_Return_Default_For_Empty_String()
            {
                // Act
                var result = string.Empty.GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual(0, result);
            }

            [Test]
            public void Should_Return_Default_For_Whitespace()
            {
                // Act
                var result = "   ".GetApprenticeshipType();
                
                // Assert
                ClassicAssert.AreEqual(0, result);
            }

            [Test]
            public void Should_Return_Custom_Default_When_Specified()
            {
                // Arrange
                const int customDefault = 99;
                
                // Act
                var result = InvalidTypeName.GetApprenticeshipType(customDefault);
                
                // Assert
                ClassicAssert.AreEqual(customDefault, result);
            }

            [Test]
            public void Should_Handle_Numeric_Strings()
            {
                // Act & Assert
                ClassicAssert.AreEqual(0, "0".GetApprenticeshipType(-1));
                ClassicAssert.AreEqual(1, "1".GetApprenticeshipType(-1));
            }
        }
    }
}