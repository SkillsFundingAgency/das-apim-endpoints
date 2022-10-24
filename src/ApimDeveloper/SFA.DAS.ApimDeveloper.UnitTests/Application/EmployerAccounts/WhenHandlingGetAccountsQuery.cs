using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.EmployerAccounts
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
            
            accountsApiClient.Setup(x => x.GetEmployerAccounts(query.UserId, query.Email)).ReturnsAsync(teamResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.UserAccountResponse.Should().BeEquivalentTo(teamResponse);
        }
    }
}