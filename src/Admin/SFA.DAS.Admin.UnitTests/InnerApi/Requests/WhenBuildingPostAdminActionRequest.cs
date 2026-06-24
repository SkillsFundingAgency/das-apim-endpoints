using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.InnerApi.Requests;

namespace SFA.DAS.Admin.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingPostAdminActionRequest
    {
        [Test]
        public void Then_Data_is_set_and_PostUrl_is_correct()
        {
            var req = new PostAdminActionRequest("bob", "Viewed", 123);

            req.Data.Should().NotBeNull();
            req.Data.Username.Should().Be("bob");
            req.Data.Action.Should().Be("Viewed");
            req.Data.UserActionId.Should().Be(123);

            req.PostUrl.Should().Be("api/users/adminactions");
        }
    }
}
