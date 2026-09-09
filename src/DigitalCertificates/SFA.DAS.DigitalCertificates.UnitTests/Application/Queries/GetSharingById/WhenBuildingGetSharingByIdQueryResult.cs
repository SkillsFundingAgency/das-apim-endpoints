using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharingById
{
    public class WhenBuildingGetSharingByIdQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(GetSharingByIdResponse response)
        {
            // Arrange & Act
            var result = new GetSharingByIdQueryResult { Response = response };

            // Assert
            result.Response.Should().BeEquivalentTo(response);
        }
    }
}
