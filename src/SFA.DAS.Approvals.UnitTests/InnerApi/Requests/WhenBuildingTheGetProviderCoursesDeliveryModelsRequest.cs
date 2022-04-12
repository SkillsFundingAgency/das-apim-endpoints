using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProviderCoursesDeliveryModelsRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            long providerId,
            string trainingCode
            )
        {
            var actual = new GetProviderCoursesDeliveryModelsRequest(providerId, trainingCode);
            
            //Assert
            actual.GetUrl.Should().Be($"providers/{providerId}/courses/{trainingCode}");
        }
    }
}