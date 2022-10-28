﻿using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ProviderRelationships.Api.Models;
using SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Models;

public class WhenCastingGetUserAccountsApiResponseFromMediatorType
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped(GetAccountsQueryResult source)
    {
        var actual = (GetUserAccountsApiResponse) source;
            
        actual.UserAccounts.Should().BeEquivalentTo(source.UserAccountResponse);
    }
        
        
    [Test]
    public void Then_If_Null_Then_Empty_Returned()
    {
        var actual = (GetUserAccountsApiResponse) (GetAccountsQueryResult)null;
            
        actual.UserAccounts.Should().BeEmpty();
    }
}