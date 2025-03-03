using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Providers.Queries;
public class WhenConvertingProviderToRoatpProvider
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
