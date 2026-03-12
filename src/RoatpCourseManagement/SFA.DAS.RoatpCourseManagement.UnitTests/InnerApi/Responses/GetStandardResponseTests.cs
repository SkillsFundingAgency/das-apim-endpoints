using System;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Responses;

[TestFixture]
public class GetStandardResponseTests
{
    [Test, AutoData]
    public void ImplicitConversion_FromCoursesApi_MapsAllProperties(GetStandardResponseFromCoursesApi source)
    {
        // Act
        GetStandardResponse result = source;

        // Assert
        result.StandardUId.Should().Be(source.StandardUId);
        result.IfateReferenceNumber.Should().Be(source.IfateReferenceNumber);
        result.LarsCode.Should().Be(source.LarsCode);
        result.Title.Should().Be(source.Title);
        result.Level.Should().Be(source.Level);
        result.ApprenticeshipType.Should().Be(source.LearningType);
        result.ApprovalBody.Should().Be(source.ApprovalBody);
        result.Route.Should().Be(source.Route);
        result.IsRegulatedForProvider.Should().Be(source.IsRegulatedForProvider);
        result.Duration.Should().Be(source.ApprenticeshipFunding.FirstOrDefault().Duration);
        result.DurationUnits.Should().Be(source.ApprenticeshipFunding.FirstOrDefault().DurationUnits);
        result.CourseType.Should().Be(source.CourseType);
    }

    [Test, AutoData]
    public void ImplicitConversion_FromCourseManagementApi_MapsAllProperties(GetStandardResponseFromCourseManagementApi source)
    {
        // Act
        GetStandardResponse result = source;

        // Assert
        result.StandardUId.Should().BeNull();
        result.IfateReferenceNumber.Should().Be(source.IfateReferenceNumber);
        result.LarsCode.Should().Be(source.LarsCode);
        result.Title.Should().Be(source.Title);
        result.Level.Should().Be(source.Level);
        result.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
        result.ApprovalBody.Should().Be(source.ApprovalBody);
        result.Route.Should().Be(source.Route);
        result.IsRegulatedForProvider.Should().Be(source.IsRegulatedForProvider);
        result.Duration.Should().Be(source.Duration);
        result.DurationUnits.Should().Be(source.DurationUnits);
        result.CourseType.Should().Be(source.CourseType);
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_NullSource_ThrowsNullReferenceException()
    {
        // Arrange
        GetStandardResponseFromCoursesApi source = null;

        // Act
        var result = (GetStandardResponse)source;

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void ImplicitConversion_FromCourseManagementApi_NullSource_ThrowsNullReferenceException()
    {
        // Arrange
        GetStandardResponseFromCourseManagementApi source = null;

        // Act
        Action act = () => { var _ = (GetStandardResponse)source; };

        // Assert
        act.Should().Throw<NullReferenceException>();
    }
}