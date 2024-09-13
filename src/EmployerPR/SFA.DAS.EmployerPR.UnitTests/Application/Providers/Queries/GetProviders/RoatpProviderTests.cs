using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Roatp.Queries.GetRoatpProviders;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Providers.Queries.GetProviders;
public class RoatpProviderTests
{
    [Test, AutoData]
    public void Operator_ConvertsFrom_ProviderDetails(Provider source)
    {
        RoatpProvider sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Ukprn, Is.EqualTo(source.Ukprn));
            Assert.That(sut.Name, Is.EqualTo(source.Name));
        });
    }
}
