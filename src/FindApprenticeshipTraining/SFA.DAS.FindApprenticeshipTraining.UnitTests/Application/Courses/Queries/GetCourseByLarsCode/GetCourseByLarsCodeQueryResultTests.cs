using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Domain;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class GetCourseByLarsCodeQueryResultTests
{
    [Test, MoqAutoData]
    public void WhenImplicitConversionFromStandardDetailsLookupResponse_ThenMapsAllProperties(GetCourseLookupResponse response)
    {
        GetCourseByLarsCodeQueryResult sut = response;

        using (new AssertionScope())
        {
            sut.StandardUId.Should().Be(response.StandardUId);
            sut.IFateReferenceNumber.Should().Be(response.IfateReferenceNumber);
            sut.LarsCode.Should().Be(response.LarsCode);
            sut.Title.Should().Be(response.Title);
            sut.Level.Should().Be(response.Level);
            sut.Version.Should().Be(response.Version);
            sut.OverviewOfRole.Should().Be(response.OverviewOfRole);
            sut.Route.Should().Be(response.Route);
            sut.RouteCode.Should().Be(response.RouteCode);
            sut.TypicalJobTitles.Should().Be(response.TypicalJobTitles);
            sut.StandardPageUrl.Should().Be(response.StandardPageUrl);
            sut.CourseType.Should().Be(response.CourseType);
            sut.LearningType.Should().Be(response.LearningType);
            sut.CourseDates.Should().Be(response.CourseDates);
        }
    }

    [Test]
    public void WhenImplicitConversionFromLookupResponse_AndCourseDatesProvided_ThenMapsCourseDates()
    {
        var courseDates = new StandardDate
        {
            LastDateStarts = new DateTime(2027, 7, 16, 0, 0, 0, DateTimeKind.Utc),
            EffectiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var response = new GetCourseLookupResponse
        {
            CourseDates = courseDates
        };

        GetCourseByLarsCodeQueryResult sut = response;

        using (new AssertionScope())
        {
            sut.CourseDates.Should().NotBeNull();
            sut.CourseDates.LastDateStarts.Should().Be(courseDates.LastDateStarts);
            sut.CourseDates.EffectiveFrom.Should().Be(courseDates.EffectiveFrom);
        }
    }

    [Test]
    public void WhenImplicitConversionFromActiveStandardWithRelatedOccupations_ThenMapsIsActiveAndRelatedOccupations()
    {
        var response = new GetCourseLookupResponse
        {
            CourseDates = new StandardDate
            {
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(30)
            },
            RelatedOccupations =
            [
                new RelatedOccupation("Occupation 1", 2),
                new RelatedOccupation("Occupation 2", 3)
            ]
        };

        GetCourseByLarsCodeQueryResult sut = response;

        using (new AssertionScope())
        {
            sut.RelatedOccupations.Should().HaveCount(2);
            sut.RelatedOccupations[0].Title.Should().Be("Occupation 1");
            sut.RelatedOccupations[0].Level.Should().Be(2);
            sut.RelatedOccupations[1].Title.Should().Be("Occupation 2");
            sut.RelatedOccupations[1].Level.Should().Be(3);
        }
    }
}
