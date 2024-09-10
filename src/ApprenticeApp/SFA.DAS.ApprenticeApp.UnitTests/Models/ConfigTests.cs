using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeApp.UnitTests.InnerApi.ApprenticeAccounts.Requests
{
    public class ConfigTests
    {
        [Test]
        public void ApprenticeProgressApiConfiguration_test()
        {
            var sut = new ApprenticeProgressApiConfiguration
            {
                Url = "url",
                Identifier = "id"
            };

            ClassicAssert.AreEqual("url", sut.Url);
            ClassicAssert.AreEqual("id", sut.Identifier);
        }
    }
}
