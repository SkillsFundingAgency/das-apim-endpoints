using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingAccountIdentifierModelFromRequestHeaderValue
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Employer(string accountId)
        {
            var accountIdentifier = $"Employer|{accountId}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Employer);
            actual.Ukprn.Should().BeNull();
            actual.AccountPublicHashedId.Should().Be(accountId);
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Provider(int ukprn)
        {
            var accountIdentifier = $"Provider|{ukprn}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().Be(ukprn);
            actual.AccountPublicHashedId.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_The_Null_Set_For_Ukprn_If_Not_In_Correct_Format(string ukprn)
        {
            var accountIdentifier = $"Provider|{ukprn}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().BeNull();
            actual.AccountPublicHashedId.Should().BeNull();
        }

        [Test, AutoData]
        public void Then_If_Not_Recognised_Format_Not_Know_Type_Returned(string accountIdentifier)
        {
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_If_Not_Recognised_AccountType_Format_Not_Know_Type_Returned(string accountId)
        {
            var accountIdentifier = $"Citizen|{accountId}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
    }
}