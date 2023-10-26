using AutoFixture.NUnit3;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.EmployerAan.InnerApi.MyApprenticeships;

namespace SFA.DAS.EmployerAan.Application.UnitTests.MyApprenticeships.Queries.GetMyApprenticeships;

public class MyApprenticeshipTests
{
    [Test]
    [AutoData]
    public void Operator_ConvertsFrom_MyApprenticeshipResponse(GetMyApprenticeshipResponse source)
    {
        GetMyApprenticeshipQueryResult sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Uln, Is.EqualTo(source.Uln));
            Assert.That(sut.ApprenticeshipId, Is.EqualTo(source.ApprenticeshipId));
            Assert.That(sut.EmployerName, Is.EqualTo(source.EmployerName));
            Assert.That(sut.StartDate, Is.EqualTo(source.StartDate));
            Assert.That(sut.EndDate, Is.EqualTo(source.EndDate));
            Assert.That(sut.TrainingProviderId, Is.EqualTo(source.TrainingProviderId));
            Assert.That(sut.TrainingProviderName, Is.EqualTo(source.TrainingProviderName));
        });
    }
}