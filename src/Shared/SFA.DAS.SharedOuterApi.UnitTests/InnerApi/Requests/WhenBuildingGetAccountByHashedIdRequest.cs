using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetAccountByHashedIdRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string hashedAccountId)
    {
        var actual = new GetAccountByHashedIdRequest(hashedAccountId);

        actual.GetUrl.Should().Be($"api/accounts/{hashedAccountId}");
    }
}