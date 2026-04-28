using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetAccountByIdRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(
        GetAccountByIdRequest actual)
    {
        actual.GetUrl.Should().Be($"api/accounts/{actual.AccountId}");
    }
}