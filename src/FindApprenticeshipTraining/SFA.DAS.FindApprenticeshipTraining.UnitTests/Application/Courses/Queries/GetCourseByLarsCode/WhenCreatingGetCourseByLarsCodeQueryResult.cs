using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class WhenCreatingGetCourseByLarsCodeQueryResult
{
    [Test]
    [MoqAutoData]
    public void Then_StandardDetailResponse_Should_Convert_Correctly(StandardDetailResponse response)
    {
        GetCourseByLarsCodeQueryResult sut = response;
        Assert.Multiple(() =>
        {
            Assert.That(sut.StandardUId, Is.EqualTo(response.StandardUId));
            Assert.That(sut.IFateReferenceNumber, Is.EqualTo(response.IfateReferenceNumber));
            Assert.That(sut.LarsCode, Is.EqualTo(response.LarsCode.ToString()));
            Assert.That(sut.Title, Is.EqualTo(response.Title));
            Assert.That(sut.Level, Is.EqualTo(response.Level));
            Assert.That(sut.Version, Is.EqualTo(response.Version));
            Assert.That(sut.OverviewOfRole, Is.EqualTo(response.OverviewOfRole));
            Assert.That(sut.Route, Is.EqualTo(response.Route));
            Assert.That(sut.RouteCode, Is.EqualTo(response.RouteCode));
            Assert.That(sut.TypicalJobTitles, Is.EqualTo(response.TypicalJobTitles));
            Assert.That(sut.StandardPageUrl, Is.EqualTo(response.StandardPageUrl));
        });
    }
}
