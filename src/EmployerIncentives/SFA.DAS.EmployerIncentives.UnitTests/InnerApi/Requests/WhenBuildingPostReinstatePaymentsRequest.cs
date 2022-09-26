using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingPostReinstatePaymentsRequest
    {
        [Test, AutoData]
        public void Then_The_Post_Url_Is_Correctly_Built(ReinstatePaymentsRequest data)
        {
            var actual = new PostReinstatePaymentsRequest(data);

            actual.PostUrl.Should().Be($"reinstate-payments");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}
