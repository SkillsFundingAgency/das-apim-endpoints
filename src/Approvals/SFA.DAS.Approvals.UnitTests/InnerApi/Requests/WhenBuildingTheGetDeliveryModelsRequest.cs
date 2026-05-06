using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetDeliveryModelsRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            long providerId,
            string trainingCode
            )
        {
            var actual = new GetDeliveryModelsRequest(providerId, trainingCode);
            
            //Assert
            actual.GetUrl.Should().Be($"api/providers/{providerId}/courses/{trainingCode}");
        }
    }
}