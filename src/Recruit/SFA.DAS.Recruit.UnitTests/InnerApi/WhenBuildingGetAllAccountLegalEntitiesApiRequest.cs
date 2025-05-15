using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi
{
    [TestFixture]
    public class WhenBuildingGetAllAccountLegalEntitiesApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Constructed(long accountId, int pageNumber, int pageSize, string sortColumn, bool isAscending)
        {
            var actual = new GetAllAccountLegalEntitiesApiRequest(accountId, pageNumber, pageSize, sortColumn, isAscending);
            actual.GetUrl.Should().Be($"api/accounts/{accountId}/legalentities/getall?pageNumber={pageNumber}&pageSize={pageSize}&sortColumn={sortColumn}&isAscending={isAscending}");
        }
    }
}