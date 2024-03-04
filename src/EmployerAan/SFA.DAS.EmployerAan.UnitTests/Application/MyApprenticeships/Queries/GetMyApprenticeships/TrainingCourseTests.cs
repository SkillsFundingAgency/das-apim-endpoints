using AutoFixture.NUnit3;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using GetFrameworkResponse = SFA.DAS.EmployerAan.InnerApi.Standards.Responses.GetFrameworkResponse;
using GetStandardResponse = SFA.DAS.EmployerAan.InnerApi.Standards.Responses.GetStandardResponse;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MyApprenticeships.Queries.GetMyApprenticeships;

public class TrainingCourseTests
{
    [Test]
    [AutoData]
    public void Operator_ConvertsFrom_GetStandardResponse(GetStandardResponse source)
    {
        TrainingCourse sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Name, Is.EqualTo(source.Title));
            Assert.That(sut.Level, Is.EqualTo(source.Level));
            Assert.That(sut.Sector, Is.EqualTo(source.Route));
            Assert.That(sut.Duration, Is.EqualTo(source.VersionDetail!.ProposedTypicalDuration));
        });
    }

    [Test]
    [AutoData]
    public void Operator_ConvertsFrom_GetFrameworkResponse(GetFrameworkResponse source)
    {
        TrainingCourse sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Name, Is.EqualTo(source.Title));
            Assert.That(sut.Level, Is.EqualTo(source.Level));
            Assert.That(sut.Sector, Is.EqualTo(source.FrameworkName));
            Assert.That(sut.Duration, Is.EqualTo(source.Duration));
        });
    }
}