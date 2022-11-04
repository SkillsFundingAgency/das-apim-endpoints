using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.EmployerAccounts.Queries;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Models
{
    public class WhenMappingUserAccountsApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetAccountsQueryResult source)
        {
            var actual = (UserAccountsApiResponse) source;
            
            actual.UserAccounts.Should().BeEquivalentTo(source.UserAccountResponse);
        }
        
        
        [Test]
        public void Then_If_Null_Then_Empty_Returned()
        {
            var actual = (UserAccountsApiResponse) (GetAccountsQueryResult)null;
            
            actual.UserAccounts.Should().BeEmpty();
        }
    }
}