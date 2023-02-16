using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.AccountUsers;

namespace SFA.DAS.Forecasting.Api.UnitTests.Models
{
    public class WhenCastingGetUserAccountsApiResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetAccountsQueryResult source)
        {
            var actual = (GetUserAccountsApiResponse) source;
            
            actual.UserAccounts.Should().BeEquivalentTo(source.UserAccountResponse);
            actual.Email.Should().Be(source.Email);
            actual.FirstName.Should().Be(source.FirstName);
            actual.LastName.Should().Be(source.LastName);
            actual.EmployerUserId.Should().Be(source.EmployerUserId);
            actual.IsSuspended.Should().Be(source.IsSuspended);
        }
        
        
        [Test]
        public void Then_If_Null_Then_Empty_Returned()
        {
            var actual = (GetUserAccountsApiResponse) (GetAccountsQueryResult)null;
            
            actual.UserAccounts.Should().BeEmpty();
            actual.Email.Should().BeNullOrEmpty();
            actual.FirstName.Should().BeNullOrEmpty();
            actual.LastName.Should().BeNullOrEmpty();
            actual.EmployerUserId.Should().BeNullOrEmpty();
            actual.IsSuspended.Should().BeFalse();
        }
    }
}