using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.AccountUsers;

namespace SFA.DAS.Recruit.Api.UnitTests.Models;

public class WhenCastingGetUserAccountsApiResponseFromMediatorType
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped(GetAccountsQueryResult source)
    {
        var actual = (GetUserAccountsApiResponse)source;

        actual.UserAccounts.Should().BeEquivalentTo(source.UserAccountResponse);
    }


    [Test]
    public void Then_If_Null_Then_Empty_Returned()
    {
        var actual = (GetUserAccountsApiResponse)(GetAccountsQueryResult)null;

        actual.UserAccounts.Should().BeEmpty();
    }
}