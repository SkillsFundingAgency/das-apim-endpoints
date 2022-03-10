using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.UnitTests.Models
{
    public class WhenMappingAccountIdentifierModelFromRequestHeaderValue
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Employer_And_Id_Upper_Cased()
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-Product-Sandbox";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Employer);
            actual.Ukprn.Should().BeNull();
            actual.AccountHashedId.Should().Be(accountId.ToUpper());
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Provider(int ukprn)
        {
            var accountIdentifier = $"Provider-{ukprn}-Product";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().Be(ukprn);
            actual.AccountHashedId.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_External(Guid externalId)
        {
            var accountIdentifier = $"External-{externalId}-Product";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.External);
            actual.ExternalId.Should().Be(externalId);
            actual.AccountHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_Unknown_If_External_Id_Is_Not_In_Correct_Format(string externalId)
        {
            var accountIdentifier = $"External-{externalId}-Product";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.ExternalId.Should().BeEmpty();
            actual.AccountHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
        [Test, AutoData]
        public void Then_The_Null_Set_For_Ukprn_If_Not_In_Correct_Format()
        {
            var ukprn = "ABC123";
            var accountIdentifier = $"Provider-{ukprn}-Product";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Provider);
            actual.Ukprn.Should().BeNull();
            actual.AccountHashedId.Should().BeNull();
        }

        [Test]
        [InlineAutoData("")]
        [InlineAutoData("Identifier")]
        [InlineAutoData("Identifier-NotValid")]
        public void Then_If_Not_Recognised_Format_Not_Known_Type_Returned(string accountIdentifier)
        {
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
        
        [Test]
        public void Then_If_No_Value_Supplied_Not_Known_Type_Returned()
        {
            var actual = new AccountIdentifier(null);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_If_Not_Recognised_AccountType_Format_Not_Know_Type_Returned()
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Citizen-{accountId}-Product";
            
            var actual = new AccountIdentifier(accountIdentifier);

            actual.AccountType.Should().Be(AccountType.Unknown);
            actual.AccountHashedId.Should().BeNull();
            actual.Ukprn.Should().BeNull();
        }
    }
}