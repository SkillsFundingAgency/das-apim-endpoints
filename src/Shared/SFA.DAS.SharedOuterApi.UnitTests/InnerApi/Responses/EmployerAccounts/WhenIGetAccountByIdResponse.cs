using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses.EmployerAccounts
{
    [TestFixture]
    public class WhenIGetAccountByIdResponse
    {
        [Test]
        public void GetAccountByIdResponse_PropertiesShouldBeSet()
        {
            // Arrange
            var response = new GetAccountByIdResponse();

            // Act & Assert
            response.AccountId.Should().BeGreaterOrEqualTo(0);
            response.HashedAccountId.Should().BeNull();
            response.PublicHashedAccountId.Should().BeNull();
            response.DasAccountName.Should().BeNull();            
            response.OwnerEmail.Should().BeNull();
            response.ApprenticeshipEmployerType.Should().BeEquivalentTo(ApprenticeshipEmployerType.NonLevy);
        }
    }
}