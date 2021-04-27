using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetAccountOwners;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetAccountOwnersQuery
    {
        [Test, MoqAutoData]
        public async Task Then_a_list_of_user_data_with_Owner_role_is_returned(
            GetAccountOwnersQuery query,
            [Frozen] Mock<IAccountsService> accountsService,
            GetAccountOwnersHandler handler,
            Fixture fixture
            )
        {
            var applicationResponse = fixture.CreateMany<UserDetails>(5).ToList();
            applicationResponse[0].Role = "None";
            applicationResponse[1].Role = "Owner";
            applicationResponse[2].Role = "Transactor";
            applicationResponse[3].Role = "Viewer";
            applicationResponse[4].Role = "Owner";

            accountsService.Setup(x => x.GetAccountUsers(query.HashedAccountId)).ReturnsAsync(applicationResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.UserDetails.Should().BeEquivalentTo(new [] {applicationResponse[1], applicationResponse[4]});
        }
    }
}