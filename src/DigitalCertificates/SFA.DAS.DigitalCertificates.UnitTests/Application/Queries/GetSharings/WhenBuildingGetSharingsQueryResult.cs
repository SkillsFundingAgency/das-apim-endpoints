using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharings
{
    public class WhenBuildingGetSharingsQueryResult
    {
        [Test, AutoData]
        public void Then_Properties_Can_Be_Set(GetSharingsResponse response)
        {
            // Arrange & Act
            var result = new GetSharingsQueryResult
            {
                Response = response
            };

            // Assert
            result.Response.Should().Be(response);
        }
    }
}