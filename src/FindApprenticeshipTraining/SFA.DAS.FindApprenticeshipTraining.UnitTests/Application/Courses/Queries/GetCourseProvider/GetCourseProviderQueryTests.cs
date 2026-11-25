using System;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderQueryTests
{
    [Test]
    public void Then_The_Constructor_Sets_All_Properties_Correctly()
    {
        long expectedUkprn = 10012345;
        var expectedLarsCode = "123";
        Guid expectedShortlistUserId = Guid.NewGuid();
        string expectedLocationName = "Manchester";
        int expectedDistance = 10;

        var sut = new GetCourseProviderQuery(
            expectedUkprn,
            expectedLarsCode,
            expectedShortlistUserId,
            expectedLocationName,
            expectedDistance
        );

        Assert.Multiple(() =>
        {
            Assert.That(sut.Ukprn, Is.EqualTo(expectedUkprn));
            Assert.That(sut.LarsCode, Is.EqualTo(expectedLarsCode));
            Assert.That(sut.ShortlistUserId, Is.EqualTo(expectedShortlistUserId));
            Assert.That(sut.LocationName, Is.EqualTo(expectedLocationName));
            Assert.That(sut.Distance, Is.EqualTo(expectedDistance));
        });
    }
}
