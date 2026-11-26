using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.UkrlpData.Queries.GetUkrlpProviders;

public class GetUkrlpProvidersQueryResultTests
{
    [Test, AutoData]
    public void ImplicitOperator_ConvertsFromUkrlpResponse(UkrlpProviderModel source)
    {
        ProviderDetails sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Ukprn, Is.EqualTo(source.Ukprn));
            Assert.That(sut.LegalName, Is.EqualTo(source.LegalName));
            Assert.That(sut.TradingName, Is.EqualTo(source.TradingName));
            Assert.That(sut.LegalAddress, Is.EqualTo(source.LegalAddress));
            Assert.That(sut.PrimaryContact, Is.EqualTo(source.ContactDetails));
        });
    }
}