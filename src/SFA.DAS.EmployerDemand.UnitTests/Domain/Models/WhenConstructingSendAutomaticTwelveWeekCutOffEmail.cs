using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.UnitTests.Domain.Models
{
    public class WhenConstructingSendAutomaticTwelveWeekCutOffEmail
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Set_Correctly(string recipientEmail, string employerName, string standardName,
            int standardLevel, string location, int numberOfApprentices, string startSharingUrl)
        {
            //Arrange
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDStartSharingURL", startSharingUrl}
            };
            
            //Act
            var actual = new StopSharingAutomaticTwelveWeekCutOffEmail(recipientEmail, employerName, standardName, standardLevel, location, numberOfApprentices, startSharingUrl);
            
            //Assert
            actual.TemplateId.Should().Be(EmailConstants.StopSharingAutomaticTwelveWeekCutOffTemplateId);
            actual.Subject.Should().Be("We’ve stopped sharing your interest in apprenticeship training with training providers");
            actual.Tokens.Should().BeEquivalentTo(expectedTokens);
            actual.RecipientAddress.Should().Be(recipientEmail);
        }
    }
}