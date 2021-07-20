using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingSendBankDetailsReEmailRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Build(long accountId, long accountLegalEntityId, string emailAddress, string addBankDetailsUrl, string baseUrl)
        {
            var request = new SendBankDetailsEmailRequest(accountId, accountLegalEntityId, emailAddress, addBankDetailsUrl);
            var actual = new PostBankDetailsRequiredEmailRequest { Data = request };

            request.AccountId.Should().Be(accountId);
            request.AccountLegalEntityId.Should().Be(accountLegalEntityId);
            request.EmailAddress.Should().Be(emailAddress);
            request.AddBankDetailsUrl.Should().Be(addBankDetailsUrl);
            actual.PostUrl.Should().Be("api/EmailCommand/bank-details-required");
        }
    }
}