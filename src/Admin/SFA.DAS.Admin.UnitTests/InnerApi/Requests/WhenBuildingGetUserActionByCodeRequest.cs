using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.InnerApi.Requests;

namespace SFA.DAS.Admin.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingGetUserActionByCodeRequest
    {
        [Test]
        public void Then_Code_is_set_and_GetUrl_is_correct()
        {
            var req = new GetUserActionByCodeRequest("ABC123");

            req.Code.Should().Be("ABC123");
            req.GetUrl.Should().Be("api/users/useractions/ABC123");
        }
    }
}
