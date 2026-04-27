using System;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.SharedOuterApi.Types.Domain;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class WhenCreatingGetCourseByLarsCodeQueryResult
{
    [Test, MoqAutoData]
    public void ImplicitConversion_StandardDetailsLookupResponse_MapsAllProperties(StandardDetailsLookupResponse response)
    {
        GetCourseByLarsCodeQueryResult sut = response;
        Assert.Multiple(() =>
        {
            Assert.That(sut.StandardUId, Is.EqualTo(response.StandardUId));
            Assert.That(sut.IFateReferenceNumber, Is.EqualTo(response.IfateReferenceNumber));
            Assert.That(sut.LarsCode, Is.EqualTo(response.LarsCode));
            Assert.That(sut.Title, Is.EqualTo(response.Title));
            Assert.That(sut.Level, Is.EqualTo(response.Level));
            Assert.That(sut.Version, Is.EqualTo(response.Version));
            Assert.That(sut.OverviewOfRole, Is.EqualTo(response.OverviewOfRole));
            Assert.That(sut.Route, Is.EqualTo(response.Route));
            Assert.That(sut.RouteCode, Is.EqualTo(response.RouteCode));
            Assert.That(sut.TypicalJobTitles, Is.EqualTo(response.TypicalJobTitles));
            Assert.That(sut.StandardPageUrl, Is.EqualTo(response.StandardPageUrl));
            Assert.That(sut.CourseType, Is.EqualTo(response.CourseType));
            Assert.That(sut.ApprenticeshipType, Is.EqualTo(response.LearningType));
            Assert.That(sut.IsActiveAvailable, Is.EqualTo(response.IsActiveAvailable));
        });
    }

    [Test]
    public void ImplicitConversion_ActiveStandardWithRelatedOccupations_MapsIsActiveAndRelatedOccupations()
    {
        var response = new StandardDetailsLookupResponse
        {
            CourseDates = new CourseDate
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

        Assert.Multiple(() =>
        {
            Assert.That(sut.IsActiveAvailable, Is.True);
            Assert.That(sut.RelatedOccupations.Count, Is.EqualTo(2));
            Assert.That(sut.RelatedOccupations[0].Title, Is.EqualTo("Occupation 1"));
            Assert.That(sut.RelatedOccupations[0].Level, Is.EqualTo(2));
            Assert.That(sut.RelatedOccupations[1].Title, Is.EqualTo("Occupation 2"));
            Assert.That(sut.RelatedOccupations[1].Level, Is.EqualTo(3));
        });
    }
}
