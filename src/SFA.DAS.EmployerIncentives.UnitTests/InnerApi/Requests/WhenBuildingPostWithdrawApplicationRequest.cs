using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostWithdrawApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_Post_Url_Is_Correctly_Built(WithdrawRequest withdrawRequest)
        {
            var actual = new PostWithdrawApplicationRequest(withdrawRequest);

            actual.PostUrl.Should().Be($"withdrawals");
            actual.Data.Should().BeEquivalentTo(withdrawRequest);
        }
    }
}