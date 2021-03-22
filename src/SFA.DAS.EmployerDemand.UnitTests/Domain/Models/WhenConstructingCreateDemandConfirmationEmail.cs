using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.UnitTests.Domain.Models
{
    public class WhenConstructingCreateDemandConfirmationEmail
    {
        [Test, AutoData]
        public void Then_Values_Are_Set_Correctly(
            string recipientEmail, 
            string recipientName,
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", recipientName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices.ToString() }
            };

            var email = new CreateDemandConfirmationEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices);

            email.TemplateId.Should().Be(EmailConstants.CreateDemandConfirmationTemplateId);
            email.RecipientAddress.Should().Be(recipientEmail);
            email.ReplyToAddress.Should().Be(EmailConstants.ReplyToAddress);
            email.Subject.Should().Be("We’ve shared your interest in apprenticeship training with training providers");
            email.Tokens.Should().BeEquivalentTo(expectedTokens);
        }

        //todo: and no number of apprentices
    }
}