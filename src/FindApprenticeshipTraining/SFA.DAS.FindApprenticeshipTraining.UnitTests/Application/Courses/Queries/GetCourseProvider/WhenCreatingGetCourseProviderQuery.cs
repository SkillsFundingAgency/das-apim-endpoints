using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using System;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProvider;

public sealed class WhenCreatingGetCourseProviderQuery
{
    [Test]
    public void Then_The_Constructor_Sets_All_Properties_Correctly()
    {
        long expectedUkprn = 10012345;
        int expectedLarsCode = 123;
        Guid expectedShortlistUserId = Guid.NewGuid();
        string expectedLocation = "Manchester";
        int expectedDistance = 10;

        var sut = new GetCourseProviderQuery(
            expectedUkprn,
            expectedLarsCode,
            expectedShortlistUserId,
            expectedLocation,
            expectedDistance
        );

        Assert.Multiple(() =>
        {
            Assert.That(sut.Ukprn, Is.EqualTo(expectedUkprn));
            Assert.That(sut.LarsCode, Is.EqualTo(expectedLarsCode ));
            Assert.That(sut.ShortlistUserId, Is.EqualTo(expectedShortlistUserId));
            Assert.That(sut.Location, Is.EqualTo(expectedLocation));
            Assert.That(sut.Distance, Is.EqualTo(expectedDistance));
        });
    }
}
