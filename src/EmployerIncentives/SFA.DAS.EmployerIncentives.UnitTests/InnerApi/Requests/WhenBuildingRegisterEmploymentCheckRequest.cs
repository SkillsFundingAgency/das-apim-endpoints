using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingRegisterEmploymentCheckRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Build(RegisterEmploymentCheckCommand command)
        {

            var actual = new RegisterEmploymentCheckRequest(command);

            actual.PostUrl.Should().Be($"api/EmploymentCheck/RegisterCheck");
            actual.Data.Should().Be(command);
        }
    }
}