using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AparRegister.InnerApi.Requests;

namespace SFA.DAS.AparRegister.UnitTests.InnerApi
{
    public class WhenBuildingGetProvidersRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            var actual = new GetProvidersRequest();

            actual.GetUrl.Should().Be("api/providers");
        }
    }
}