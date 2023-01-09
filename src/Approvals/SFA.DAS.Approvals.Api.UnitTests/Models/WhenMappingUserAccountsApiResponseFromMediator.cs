using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models.AccountUsers;
using SFA.DAS.Approvals.Application.AccountUsers.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingUserAccountsApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetAccountsQueryResult source)
        {
            var actual = (UserAccountsApiResponse) source;
            
            actual.UserAccounts.Should().BeEquivalentTo(source.UserAccountResponse);
            actual.FirstName.Should().Be(source.FirstName);
            actual.LastName.Should().Be(source.LastName);
            actual.EmployerUserId.Should().Be(source.EmployerUserId);
        }
        
        
        [Test]
        public void Then_If_Null_Then_Empty_Returned()
        {
            var actual = (UserAccountsApiResponse) (GetAccountsQueryResult)null;
            
            actual.UserAccounts.Should().BeEmpty();
            actual.FirstName.Should().BeNullOrEmpty();
            actual.LastName.Should().BeNullOrEmpty();
            actual.EmployerUserId.Should().BeNullOrEmpty();
        }
    }
}