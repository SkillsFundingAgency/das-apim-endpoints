using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.UnitTests.Domain.Models
{
    public class WhenConstructingProviderIsInterestedEmail
    {
        [Test, AutoData]
        public void Then_Values_Are_Set_Correctly(
            string recipientEmail, 
            string recipientName,
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite,
            string fatUrl,
            string stopSharingUrl)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", recipientName },
                {"AEDApprenticeshipTrainingCourse", $"{standardName} (level {standardLevel})" },
                {"AEDApprenticeshipLocation", location },
                {"AEDNumberOfApprentices", numberOfApprentices.ToString() },
                {"AEDProviderName", providerName },
                {"AEDProviderEmail", providerEmail },
                {"AEDProviderTelephone", providerPhone },
                {"AEDProviderWebsite", providerWebsite },
                {"FatURL", $"You can find out more about this training provider at {fatUrl}" },
                {"AEDStopSharingURL", stopSharingUrl }
            };

            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                providerName,
                providerEmail,
                providerPhone,
                providerWebsite,
                fatUrl,
                stopSharingUrl);

            email.TemplateId.Should().Be(EmailConstants.ProviderInterestedTemplateId);
            email.RecipientAddress.Should().Be(recipientEmail);
            email.ReplyToAddress.Should().Be(EmailConstants.ReplyToAddress);
            email.Tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test, AutoData]
        public void And_No_NumberOfApprentices_Then_NotSure_Text(
            string recipientEmail, 
            string recipientName,
            int standardId,
            string standardName, 
            int standardLevel, 
            string location,
            int ukprn,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite,
            string fatUrl,
            string stopSharingUrl)
        {
            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                0,
                providerName,
                providerEmail,
                providerPhone,
                providerWebsite,
                fatUrl,
                stopSharingUrl);

            email.Tokens["AEDNumberOfApprentices"].Should().Be("Not sure");
        }

        [Test, AutoData]
        public void And_No_ProviderWebsite_Then_Alternative_Text(
            string recipientEmail, 
            string recipientName,
            string standardName, 
            int standardLevel, 
            string location,
            int numberOfApprentices,
            string providerName,
            string providerEmail,
            string providerPhone,
            string fatUrl,
            string stopSharingUrl)
        {
            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                providerName,
                providerEmail,
                providerPhone,
                null,
                fatUrl,
                stopSharingUrl);

            email.Tokens["AEDProviderWebsite"].Should().Be("-");
        }

        [Test, AutoData]
        public void And_Provider_No_FatUrl_Then_Alternative_Text(
            string recipientEmail, 
            string recipientName,
            string standardName, 
            int standardLevel, 
            string location,
            int numberOfApprentices,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite,
            string stopSharingUrl)
        {
            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                providerName,
                providerEmail,
                providerPhone,
                providerWebsite,
                null,
                stopSharingUrl);

            email.Tokens["FatURL"].Should().Be("---");
        }
    }
}
