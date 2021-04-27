using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.AccountsServiceTests
{
    public class WhenCallingGetAccountOwners
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_a_list_of_Users_associated_with_the_Account(
            string accountId,
            IEnumerable<UserDetails> apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> client,
            AccountsService service
        )
        {
            client.Setup(x =>
                    x.GetAll<UserDetails>(
                        It.Is<GetAccountUsersRequest>(c => c.GetAllUrl.Contains(accountId))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetAccountUsers(accountId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}