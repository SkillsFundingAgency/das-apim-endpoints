using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingAccountIdentifierModelFromRequestHeaderValue
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Employer_And_Id_Upper_Cased()
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Employer);
            actual.Ukprn.Should().BeNull();
            actual.AccountPublicHashedId.Should().Be(accountId.ToUpper());
            actual.IsSandbox.Should().BeFalse();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Provider(int ukprn)
        {
            var accountIdentifier = $"Provider-{ukprn}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().Be(ukprn);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.IsSandbox.Should().BeFalse();
        }
        
        [Test, AutoData]
        public void Then_The_Null_Set_For_Ukprn_If_Not_In_Correct_Format()
        {
            var ukprn = "ABC123";
            var accountIdentifier = $"Provider-{ukprn}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().BeNull();
            actual.AccountPublicHashedId.Should().BeNull();
            actual.IsSandbox.Should().BeFalse();
        }

        [Test, AutoData]
        public void Then_If_Not_Recognised_Format_Not_Known_Type_Returned(string accountIdentifier)
        {
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
            actual.IsSandbox.Should().BeFalse();
        }
        
        [Test]
        public void Then_If_No_Value_Supplied_Not_Known_Type_Returned()
        {
            var actual = new AccountIdentifier(null);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
            actual.IsSandbox.Should().BeFalse();
        }
        
        [Test, AutoData]
        public void Then_If_Not_Recognised_AccountType_Format_Not_Know_Type_Returned()
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Citizen-{accountId}";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
            actual.IsSandbox.Should().BeFalse();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Sandbox_Employer_And_Id_Upper_Cased()
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-sandbox";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Employer);
            actual.Ukprn.Should().BeNull();
            actual.AccountPublicHashedId.Should().Be(accountId.ToUpper());
            actual.IsSandbox.Should().BeTrue();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Sandbox_Provider(int ukprn)
        {
            var accountIdentifier = $"Provider-{ukprn}-sandbox";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().Be(ukprn);
            actual.AccountPublicHashedId.Should().BeNull();
            actual.IsSandbox.Should().BeTrue();
        }
    }
}