using System;
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
        result.LarsCode.Should().Be(source.LarsCode.ToString());
        result.Title.Should().Be(source.Title);
        result.Level.Should().Be(source.Level);
        result.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
        result.ApprovalBody.Should().Be(source.ApprovalBody);
        result.Route.Should().Be(source.Route);
        result.IsRegulatedForProvider.Should().Be(source.IsRegulatedForProvider);
    }

    [Test]
    public void ImplicitConversion_FromCoursesApi_NullSource_ThrowsNullReferenceException()
    {
        // Arrange
        GetStandardResponseFromCoursesApi source = null;

        // Act
        Action act = () => { var _ = (GetStandardResponse)source; };

        // Assert
        act.Should().Throw<NullReferenceException>();
    }
}