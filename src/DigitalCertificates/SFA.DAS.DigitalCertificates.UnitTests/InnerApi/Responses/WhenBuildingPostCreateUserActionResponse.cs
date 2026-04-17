using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingPostCreateUserActionResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(string actionCode)
        {
            // Arrange & Act
            var response = new PostCreateUserActionResponse
            {
                ActionCode = actionCode
            };

            // Assert
            response.ActionCode.Should().Be(actionCode);
        }
    }
}
