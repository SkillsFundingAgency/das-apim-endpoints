using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Responses;

[TestFixture]
public class GetStandardResponseTests
{
    [Test, AutoData]
    public void ImplicitConversion_WhenCoursesApiHasNoFunding_MapsPropertiesAndSetsDefaults(GetStandardResponseFromCoursesApi source)
    {
        // Arrange
        source.ApprenticeshipFunding = new List<ApprenticeshipFunding>();

        // Act
        GetStandardResponse result = source;

        result.StandardUId.Should().Be(source.StandardUId);
        result.IfateReferenceNumber.Should().Be(source.IfateReferenceNumber);
        result.LarsCode.Should().Be(source.LarsCode);
        result.Title.Should().Be(source.Title);
        result.Level.Should().Be(source.Level);
        result.ApprenticeshipType.Should().Be(source.LearningType);
        result.ApprovalBody.Should().Be(source.ApprovalBody);
        result.Route.Should().Be(source.Route);
        result.IsRegulatedForProvider.Should().Be(source.IsRegulatedForProvider);

        result.Duration.Should().Be(0);
        result.DurationUnits.Should().Be(default(DurationUnits));

        result.CourseType.Should().Be(source.CourseType);
    }

    [Test, AutoData]
    public void ImplicitConversion_WhenCoursesApiHasFunding_MapsPropertiesAndUsesMostRecentFunding(GetStandardResponseFromCoursesApi source)
    {
        // Arrange
        var older = new ApprenticeshipFunding { EffectiveFrom = new DateTime(2020, 1, 1), Duration = 74, DurationUnits = DurationUnits.Months };
        var newer = new ApprenticeshipFunding { EffectiveFrom = new DateTime(2022, 1, 1), Duration = 167, DurationUnits = DurationUnits.Hours };

        // Arrange
        source.ApprenticeshipFunding = new List<ApprenticeshipFunding> { older, newer };

        // Act
        var result = (GetStandardResponse)source;

        // Assert - properties are mapped
        result.StandardUId.Should().Be(source.StandardUId);
        result.IfateReferenceNumber.Should().Be(source.IfateReferenceNumber);
        result.LarsCode.Should().Be(source.LarsCode);
        result.Title.Should().Be(source.Title);
        result.Level.Should().Be(source.Level);
        result.ApprenticeshipType.Should().Be(source.LearningType);
        result.ApprovalBody.Should().Be(source.ApprovalBody);
        result.Route.Should().Be(source.Route);
        result.IsRegulatedForProvider.Should().Be(source.IsRegulatedForProvider);
        result.CourseType.Should().Be(source.CourseType);

        // Assert funding chosen is the most recent (by EffectiveFrom)
        result.Duration.Should().Be(newer.Duration);
        result.DurationUnits.Should().Be(newer.DurationUnits);
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_NullSource_ReturnsNull()
    {
        // Arrange
        GetStandardResponseFromCoursesApi source = null;

        // Act
        GetStandardResponse result = source;

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsNullAndEffectiveFromIsNull_IsActiveAvailableReturnsFalse()
    {
        // Arrange
        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = null,
                EffectiveFrom = null,
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsNullAndEffectiveFromIsPastDate_IsActiveAvailableReturnsTrue()
    {
        // Arrange
        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = null,
                EffectiveFrom = dateTimeNow.AddDays(-1),
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsFutureDateAndNotSameAsEffectiveFrom_IsActiveAvailableReturnsTrue()
    {
        // Arrange
        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = dateTimeNow
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsFutureDateAndEffectiveFromIsNull_IsActiveAvailableReturnsFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = null
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsFutureDateAndIsSameAsEffectiveFrom_IsActiveAvailableReturnsFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = dateTimeNow.AddDays(1),
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsPastDate_IsActiveAvailableReturnsFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(-1),
                EffectiveFrom = dateTimeNow
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsToday_IsActiveAvailableReturnsTrue()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow,
                EffectiveFrom = dateTimeNow.AddDays(-1)
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_EffectiveFromIsToday_IsActiveAvailableReturnsTrue()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = dateTimeNow,
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_EffectiveFromIsFutureDate_IsActiveAvailableReturnsFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(2),
                EffectiveFrom = dateTimeNow.AddDays(1),
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_LastDateStartsIsNullEffectiveFromIsFutureDate_IsActiveAvailableReturnsFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow.Date;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = null,
                EffectiveFrom = dateTimeNow.AddDays(1),
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_CourseDatesIsNull_IsActiveAvailableReturnsFalse()
    {
        // Arrange
        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = null
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_WhenLastDateStartsIsNull_ReturnsIsActiveAvailableTrue()
    {
        // Arrange
        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = null,
                EffectiveFrom = null,
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_WhenLastDateStartsIsFutureAndDifferentFromEffectiveFrom_ReturnsIsActiveAvailableTrue()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = dateTimeNow
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_WhenLastDateStartsIsFutureAndEffectiveFromIsNull_ReturnsIsActiveAvailableTrue()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = null
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }

    [Test]
    public void ImplicitConversion_WhenLastDateStartsIsFutureAndSameAsEffectiveFrom_ReturnsIsActiveAvailableFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(1),
                EffectiveFrom = dateTimeNow.AddDays(1),
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_WhenLastDateStartsIsPast_ReturnsIsActiveAvailableFalse()
    {
        // Arrange

        DateTime dateTimeNow = DateTime.UtcNow;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = dateTimeNow.AddDays(-1),
                EffectiveFrom = dateTimeNow
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeFalse();
    }

    [Test]
    public void ImplicitConversion_WhenCourseDatesIsNull_ReturnsIsActiveAvailableTrue()
    {
        // Arrange
        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = null
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().BeTrue();
    }
}
