using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetOrganisationResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(string endPointAssessorName)
        {
            var response = new GetOrganisationResponse
            {
                EndPointAssessorName = endPointAssessorName
            };

            response.EndPointAssessorName.Should().Be(endPointAssessorName);
        }
    }
}
