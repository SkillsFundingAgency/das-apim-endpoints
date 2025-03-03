using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.AccountUsers.Queries;

namespace SFA.DAS.Reservations.Api.UnitTests.Models
{
    public class WhenCastingGetUserAccountsApiResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetAccountsQueryResult source)
        {
            var actual = (GetUserAccountsApiResponse) source;
            
            actual.UserAccounts.Should().BeEquivalentTo(source.UserAccountResponse);
            actual.UserId.Should().Be(source.UserId);
            actual.IsSuspended.Should().Be(source.IsSuspended);
        }
        
        
        [Test]
        public void Then_If_Null_Then_Empty_Returned()
        {
            var actual = (GetUserAccountsApiResponse) (GetAccountsQueryResult)null;
            
            actual.UserAccounts.Should().BeEmpty();
            actual.UserId.Should().BeNullOrEmpty();
            actual.IsSuspended.Should().BeFalse();
        }
    }
}