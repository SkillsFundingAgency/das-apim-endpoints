using AutoFixture.NUnit3;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetUkrlpRequestTests
{
    [Test, AutoData]
    public void GetUrl_ReturnsCorrectUrl(int ukprn)
    {
        var expected = $"/ukrlp/providers/{ukprn}";
        GetUkrlpRequest sut = new(ukprn);

        var actual = sut.GetUrl;

        Assert.That(actual, Is.EqualTo(expected));
    }
}
