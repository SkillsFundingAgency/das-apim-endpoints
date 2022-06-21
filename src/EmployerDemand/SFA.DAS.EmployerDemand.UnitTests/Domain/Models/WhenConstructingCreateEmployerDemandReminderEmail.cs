using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.UnitTests.Domain.Models
{
    public class WhenConstructingCreateEmployerDemandReminderEmail
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Set_Correctly(
            string recipientEmail, 
            string employerName, 
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices,
            string stopSharingUrl)
        {
            //Arrange
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices > 0 ? numberOfApprentices.ToString() : "Not sure" },
                {"AEDStopSharingURL", stopSharingUrl}
            };
            
            //Act
            var actual = new CreateEmployerDemandReminderEmail(
                recipientEmail, 
                employerName, 
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                stopSharingUrl);
            
            //Assert
            actual.TemplateId.Should().Be(EmailConstants.EmployerDemandReminderTemplateId);
            actual.Tokens.Should().BeEquivalentTo(expectedTokens);
            actual.RecipientAddress.Should().Be(recipientEmail);
        }
    }
}