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
    public void ImplicitConversion_FromCoursesApiNoFunding_MapsPropertiesAndSetsDefaults(GetStandardResponseFromCoursesApi source)
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
    public void ImplicitConversion_FromCoursesApiWithFunding_MapsPropertiesAndUsesMostRecentFunding(GetStandardResponseFromCoursesApi source)
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

    [TestCase(null, null, false, TestName = "LastDateStartsIsNullAndEffectiveFromIsNull")]
    [TestCase(null, -1, true, TestName = "LastDateStartsIsNullAndEffectiveFromIsPastDate")]
    [TestCase(1, 0, true, TestName = "LastDateStartsIsFutureDateAndNotSameAsEffectiveFrom")]
    [TestCase(1, null, false, TestName = "LastDateStartsIsFutureDateAndEffectiveFromIsNull")]
    [TestCase(1, 1, false, TestName = "LastDateStartsIsFutureDateAndIsSameAsEffectiveFrom")]
    [TestCase(-1, 0, false, TestName = "LastDateStartsIsPastDate")]
    [TestCase(0, -1, true, TestName = "LastDateStartsIsToday")]
    [TestCase(1, 0, true, TestName = "EffectiveFromIsToday")]
    [TestCase(1, 2, false, TestName = "EffectiveFromIsFutureDate")]
    [TestCase(null, 1, false, TestName = "LastDateStartsIsNullEffectiveFromIsFutureDate")]
    public void ImplicitConversion_FromCoursesApi_IsActiveAvailable_TestCases(
        int? lastDateStartsNoOfDays,
        int? effectiveFromNoOfDays,
        bool expectedIsActiveAvailable)
    {
        // Arrange
        DateTime dateTimeNow = DateTime.UtcNow.Date;

        DateTime? lastDateStarts = lastDateStartsNoOfDays.HasValue ? dateTimeNow.AddDays(lastDateStartsNoOfDays.Value) : (DateTime?)null;
        DateTime? effectiveFrom = effectiveFromNoOfDays.HasValue ? dateTimeNow.AddDays(effectiveFromNoOfDays.Value) : (DateTime?)null;

        var source = new GetStandardResponseFromCoursesApi()
        {
            CourseDates = new CourseDates()
            {
                LastDateStarts = lastDateStarts,
                EffectiveFrom = effectiveFrom,
            }
        };

        // Act
        GetStandardResponse result = source;

        // Assert
        result.IsActiveAvailable.Should().Be(expectedIsActiveAvailable);
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
}