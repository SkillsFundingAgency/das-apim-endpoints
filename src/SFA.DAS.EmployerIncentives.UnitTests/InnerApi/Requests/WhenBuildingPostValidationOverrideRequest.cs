using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostValidationOverrideRequest
    {
        [Test, AutoData]
        public void Then_The_Post_Url_Is_Correctly_Built(ValidationOverrideRequest validationOverrideRequest)
        {
            var actual = new PostValidationOverrideRequest(validationOverrideRequest);

            actual.PostUrl.Should().Be($"validation-overrides");
            actual.Data.Should().BeEquivalentTo(validationOverrideRequest);
        }
    }
}