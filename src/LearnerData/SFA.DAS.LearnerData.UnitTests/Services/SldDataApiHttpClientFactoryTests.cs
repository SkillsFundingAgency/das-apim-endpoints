using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Services;

[TestFixture]
public class SldDataApiHttpClientFactoryTests
{
    [TestCase("")]
    [TestCase("anything")]
    public void Then_every_client_requested_is_the_named_sld_data_api_client(string requestedName)
    {
        using var expected = new HttpClient();
        var inner = new Mock<IHttpClientFactory>();
        inner.Setup(x => x.CreateClient(SldDataApiHttpClientFactory.ClientName)).Returns(expected);

        var actual = new SldDataApiHttpClientFactory(inner.Object).CreateClient(requestedName);

        actual.Should().BeSameAs(expected);
    }
}
