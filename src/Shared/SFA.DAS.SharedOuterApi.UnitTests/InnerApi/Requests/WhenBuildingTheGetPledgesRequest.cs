using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetPledgesRequest
    {
        [Test, AutoData]
        public void And_No_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built()
        {
            var actual = new GetPledgesRequest();

            Assert.That($"pledges?page=1", Is.EqualTo(actual.GetUrl));
        }

        [Test, AutoData]
        public void And_The_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built(long accountId)
        {
            var actual = new GetPledgesRequest(accountId: accountId);

            Assert.That($"pledges?accountId={accountId}&page=1", Is.EqualTo(actual.GetUrl));
        }

        [Test, AutoData]
        public void And_The_Page_Supplied_Then_The_GetUrl_Is_Correctly_Built(int page)
        {
            var actual = new GetPledgesRequest(page: page);

            Assert.That($"pledges?page={page}", Is.EqualTo(actual.GetUrl));
        }

        [Test, AutoData]
        public void And_The_PageSize_Supplied_Then_The_GetUrl_Is_Correctly_Built(int pageSize)
        {
            var actual = new GetPledgesRequest(pageSize: pageSize);

            Assert.That($"pledges?page=1&pageSize={pageSize}", Is.EqualTo(actual.GetUrl));
        }
    }
}