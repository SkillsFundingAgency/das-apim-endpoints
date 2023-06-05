using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.AccountUsers.Queries;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.AccountUsers
{
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

            actual.UserAccountResponse.Should().BeEquivalentTo(teamResponse,
                options => options
                    .Excluding(c => c.FirstName)
                    .Excluding(c => c.LastName)
                    .Excluding(c => c.UserId)
                    .Excluding(c => c.IsSuspended)
                    .Excluding(c => c.DisplayName)
            );
            actual.FirstName.Should().Be(teamResponse.FirstOrDefault().FirstName);
            actual.LastName.Should().Be(teamResponse.FirstOrDefault().LastName);
            actual.EmployerUserId.Should().Be(teamResponse.FirstOrDefault().UserId);
            actual.IsSuspended.Should().Be(teamResponse.FirstOrDefault().IsSuspended);
        }
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned_with_No_Accounts(
            GetAccountsQuery query,
            EmployerAccountUser response,
            [Frozen] Mock<IEmployerAccountsService> accountsService,
            GetAccountsQueryHandler handler)
        {
            response.Role = null;
            response.DasAccountName = null;
            response.EncodedAccountId = null;
            query.UserId = Guid.NewGuid().ToString();
            accountsService.Setup(x =>
                    x.GetEmployerAccounts(It.Is<EmployerProfile>(c =>
                        c.Email.Equals(query.Email) && c.UserId.Equals(query.UserId))))
                .ReturnsAsync(new List<EmployerAccountUser> {response});

            var actual = await handler.Handle(query, CancellationToken.None);

            
            actual.FirstName.Should().Be(response.FirstName);
            actual.LastName.Should().Be(response.LastName);
            actual.EmployerUserId.Should().Be(response.UserId);
            actual.IsSuspended.Should().Be(response.IsSuspended);
            actual.UserAccountResponse.Should().BeEmpty();
        }
    }
}