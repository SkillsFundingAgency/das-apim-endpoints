using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountByIdRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
          GetAccountByIdRequest actual)
        {
            actual.GetUrl.Should().Be($"api/accounts/{actual.AccountId}");
        }
    }
}