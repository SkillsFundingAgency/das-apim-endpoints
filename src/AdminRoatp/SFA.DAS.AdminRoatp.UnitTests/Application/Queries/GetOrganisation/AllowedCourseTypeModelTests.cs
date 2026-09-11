using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisation;

public class AllowedCourseTypeModelTests
{
    [Test, AutoData]
    public void ImplicitOperator_MapsCourseTypeToCourseTypeName(ProviderCourseTypeModel source)
    {
        // Arrange
        AllowedCourseTypeModel result = source;

        // Assert
        result.CourseTypeName.Should().Be(source.CourseType);
    }

    [Test, AutoData]
    public void ImplicitOperator_MapsAllProperties(ProviderCourseTypeModel source)
    {
        // Arrange
        AllowedCourseTypeModel result = source;

        // Assert
        result.CourseTypeId.Should().Be(source.CourseTypeId);
        result.CourseTypeName.Should().Be(source.CourseType);
        result.IsRestricted.Should().Be(source.IsRestricted);
        result.RestrictedCount.Should().Be(source.RestrictedCount);
        result.AllowedCount.Should().Be(source.AllowedCount);
    }
}
