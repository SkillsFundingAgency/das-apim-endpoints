using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeship.Queries.GetMyApprenticeship
{
    public class MyApprenticeshipTests
    {
        [Test]
        [AutoData]
        public void Operator_ConvertsFrom_MyApprenticeshipResponse(MyApprenticeshipResponse source)
        {
            Application.MyApprenticeship.Queries.GetMyApprenticeship.MyApprenticeship sut = source;

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
}
