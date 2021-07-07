using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.UnitTests.Domain.Models
{
    public class WhenConstructingVerifyContactEmailEmployerDemandEmail
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Set_Correctly(string recipientEmail, string employerName, string standardName, int standardLevel, string confirmationLink)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName},
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})"},
                {"AEDConfirmEmailURL", confirmationLink}
            };

            var actual = new VerifyEmployerDemandEmail(recipientEmail, employerName, standardName, standardLevel, confirmationLink);

            actual.TemplateId.Should().Be(EmailConstants.VerifyContactEmailEmployerDemandTemplateId);
            actual.Tokens.Should().BeEquivalentTo(expectedTokens);
            actual.RecipientAddress.Should().Be(recipientEmail);
        }
    }
}