using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingPostRevertPaymentsRequest
    {
        [Test, AutoData]
        public void Then_The_Post_Url_Is_Correctly_Built(RevertPaymentsRequest data)
        {
            var actual = new PostRevertPaymentsRequest(data);

            actual.PostUrl.Should().Be($"revert-payments");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}
