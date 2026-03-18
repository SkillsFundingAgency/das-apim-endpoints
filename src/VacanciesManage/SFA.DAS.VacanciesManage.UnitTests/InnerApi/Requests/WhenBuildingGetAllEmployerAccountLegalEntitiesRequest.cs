using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAllEmployerAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string encodedAccountId)
        {
            var actual = new GetAllEmployerAccountLegalEntitiesRequest(encodedAccountId);

            actual.GetUrl.Should().Be($"api/accounts/{encodedAccountId}");
        }
    }
}