using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{

    [TestFixture]
    public class WhenBuildingPostRecalculateEarningsRequest
    {
        [Test, AutoData]
        public void Then_The_Post_Url_Is_Correctly_Built(RecalculateEarningsRequest data)
        {
            var actual = new PostRecalculateEarningsRequest(data);

            actual.PostUrl.Should().Be($"earningsRecalculations");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}
