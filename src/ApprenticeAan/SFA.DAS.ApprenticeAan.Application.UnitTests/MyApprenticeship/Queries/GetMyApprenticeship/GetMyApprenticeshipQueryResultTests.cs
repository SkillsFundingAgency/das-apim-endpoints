using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeship.Queries.GetMyApprenticeship
{
    public class GetMyApprenticeshipQueryResultTests
    {
        [Test]
        [AutoData]
        public void Operator_ConvertsFrom_GetMyApprenticeshipsQueryResponse(GetMyApprenticeshipsQueryResponse source)
        {
            GetMyApprenticeshipQueryResult sut = source;

            Assert.Multiple(() =>
            {
                Assert.That(sut.ApprenticeId, Is.EqualTo(source.ApprenticeId));
                Assert.That(sut.DateOfBirth, Is.EqualTo(source.DateOfBirth));
                Assert.That(sut.Email, Is.EqualTo(source.Email));
                Assert.That(sut.FirstName, Is.EqualTo(source.FirstName));
                Assert.That(sut.LastName, Is.EqualTo(source.LastName));
            });
        }
    }
}
