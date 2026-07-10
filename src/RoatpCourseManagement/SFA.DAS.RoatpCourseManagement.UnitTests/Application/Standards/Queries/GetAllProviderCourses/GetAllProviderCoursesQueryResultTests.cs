using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetAllProviderCourses;

[TestFixture]
public class GetAllProviderCoursesQueryResultTests
{
    [Test, AutoData]
    public void Operator_ConvertsToGetAllProviderCoursesQueryResult(GetAllProviderCoursesResponse source)
    {
        var result = (GetAllProviderCoursesQueryResult)source;

        result.Should().BeEquivalentTo(source, option =>
        {
            option.ExcludingMissingMembers();
            return option;
        });
    }
}
