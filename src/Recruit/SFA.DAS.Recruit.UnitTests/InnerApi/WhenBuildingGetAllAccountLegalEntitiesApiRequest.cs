using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi
{
    [TestFixture]
    public class WhenBuildingGetAllAccountLegalEntitiesApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Constructed(GetAllAccountLegalEntitiesApiRequest.GetAllAccountLegalEntitiesApiRequestData payload )
        {
            var actual = new GetAllAccountLegalEntitiesApiRequest(payload);
            actual.PostUrl.Should().Be($"api/accountlegalentities/GetAll");
        }
    }
}