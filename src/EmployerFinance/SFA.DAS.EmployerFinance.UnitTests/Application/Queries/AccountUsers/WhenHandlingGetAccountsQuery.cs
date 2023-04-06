using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.AccountUsers.Queries;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            [Frozen] Mock<IEmployerAccountsService> accountService,
            GetAccountsQueryHandler handler)
        {
            query.UserId = Guid.NewGuid().ToString();

            accountService.Setup(x =>
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
            actual.FirstName.Equals(teamResponse.FirstOrDefault().FirstName);
            actual.LastName.Equals(teamResponse.FirstOrDefault().LastName);
            actual.EmployerUserId.Equals(teamResponse.FirstOrDefault().UserId);
            actual.IsSuspended.Equals(teamResponse.FirstOrDefault().IsSuspended);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Returns_Does_Not_Get_Agreement_Version_If_No_Accounts(
            GetAccountsQuery query,
            GetSignedAgreementVersionResponse apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApClient,
            [Frozen] Mock<IEmployerAccountsService> accountsService,
            GetAccountsQueryHandler handler)
        {
            query.UserId = Guid.NewGuid().ToString();
            accountsService.Setup(x =>
                    x.GetEmployerAccounts(It.Is<EmployerProfile>(c =>
                        c.Email.Equals(query.Email) && c.UserId.Equals(query.UserId))))
                .ReturnsAsync(new List<EmployerAccountUser>());
            
            var actual = await handler.Handle(query, CancellationToken.None);

            accountApClient
                .Verify(x => x.GetWithResponseCode<GetSignedAgreementVersionResponse>(
                    It.IsAny<GetSignedAgreementVersionRequest>()), Times.Never);
        }
    }
}