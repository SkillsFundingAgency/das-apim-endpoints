using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingUpdateEmploymentCheckRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Build(UpdateEmploymentCheckRequest request)
        {
            request.PutUrl.Should().Be($"employmentchecks/{request.Data.CorrelationId}");
        }
    }
}