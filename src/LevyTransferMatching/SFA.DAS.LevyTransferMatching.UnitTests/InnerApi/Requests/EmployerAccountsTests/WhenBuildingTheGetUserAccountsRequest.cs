using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.EmployerAccounts;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.EmployerAccountsTests
{
    public class WhenBuildingTheGetUserAccountsRequest
    {
        [Test, AutoData]
        public void Then_The_GetAllUrl_Is_Correctly_Built(string userId)
        {
            var actual = new GetUserAccountsRequest(userId);

            Assert.AreEqual(
                $"api/user/{userId}/accounts",
                actual.GetAllUrl);
        }
    }
}