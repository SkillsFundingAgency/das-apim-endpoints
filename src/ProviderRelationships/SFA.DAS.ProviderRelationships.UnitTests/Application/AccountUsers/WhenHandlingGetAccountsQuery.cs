﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.AccountUsers;

public class WhenHandlingGetAccountsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetAccountsQuery query,
        List<EmployerAccountUser> teamResponse,
        [Frozen] Mock<IEmployerAccountsService> accountsApiClient,
        GetAccountsQueryHandler handler)
    {
        query.UserId = Guid.NewGuid().ToString();

        accountsApiClient.Setup(x =>
                x.GetEmployerAccounts(It.Is<EmployerProfile>(c =>
                    c.Email.Equals(query.Email) && c.UserId.Equals(query.UserId))))
            .ReturnsAsync(teamResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.UserAccountResponse.Should().BeEquivalentTo(teamResponse);
    }
}