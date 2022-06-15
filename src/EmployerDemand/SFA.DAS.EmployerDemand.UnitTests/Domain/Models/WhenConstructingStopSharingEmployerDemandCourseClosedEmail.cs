using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.UnitTests.Domain.Models
{
    public class WhenConstructingStopSharingEmployerDemandCourseClosedEmail
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

            var email = new StopSharingEmployerDemandCourseClosedEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices);

            email.TemplateId.Should().Be(EmailConstants.StopSharingEmployerDemandCourseClosedTemplateId);
            email.RecipientAddress.Should().Be(recipientEmail);
            email.ReplyToAddress.Should().Be(EmailConstants.ReplyToAddress);
            email.Tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test, AutoData]
        public void And_No_NumberOfApprentices_Then_NotSure_Text(
            string recipientEmail, 
            string recipientName,
            string standardName, 
            int standardLevel, 
            string location)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", recipientName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", "Not sure" }
            };

            var email = new StopSharingEmployerDemandCourseClosedEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                0);

            email.Tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}