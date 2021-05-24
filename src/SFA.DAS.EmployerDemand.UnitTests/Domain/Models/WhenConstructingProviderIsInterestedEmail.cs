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
            int standardId,
            string standardName, 
            int standardLevel, 
            string location, 
            int numberOfApprentices,
            int ukprn,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite)
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
                {"FatURL", $"https://findapprenticeshiptraining.apprenticeships.education.gov.uk/courses/{standardId}/providers/{ukprn}" },
                {"AEDStopSharingURL", "" }
            };

            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardId,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                ukprn,
                providerName,
                providerEmail,
                providerPhone,
                providerWebsite,
                true);

            email.TemplateId.Should().Be(EmailConstants.ProviderInterestedTemplateId);
            email.RecipientAddress.Should().Be(recipientEmail);
            email.ReplyToAddress.Should().Be(EmailConstants.ReplyToAddress);
            email.Subject.Should().Be("A training provider is interested in offering your apprenticeship training");
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
            string providerWebsite)
        {
            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardId,
                standardName, 
                standardLevel, 
                location, 
                0,
                ukprn,
                providerName,
                providerEmail,
                providerPhone,
                providerWebsite,
                true);

            email.Tokens["AEDNumberOfApprentices"].Should().Be("Not sure");
        }

        [Test, AutoData]
        public void And_No_ProviderWebsite_Then_Alternative_Text(
            string recipientEmail, 
            string recipientName,
            int standardId,
            string standardName, 
            int standardLevel, 
            string location,
            int numberOfApprentices,
            int ukprn,
            string providerName,
            string providerEmail,
            string providerPhone)
        {
            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardId,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                ukprn,
                providerName,
                providerEmail,
                providerPhone,
                null,
                true);

            email.Tokens["AEDProviderWebsite"].Should().Be("-");
        }

        [Test, AutoData]
        public void And_Provider_Not_Offer_This_Course_Then_Alternative_Text(
            string recipientEmail, 
            string recipientName,
            int standardId,
            string standardName, 
            int standardLevel, 
            string location,
            int numberOfApprentices,
            int ukprn,
            string providerName,
            string providerEmail,
            string providerPhone,
            string providerWebsite)
        {
            var email = new ProviderIsInterestedEmail(
                recipientEmail, 
                recipientName,
                standardId,
                standardName, 
                standardLevel, 
                location, 
                numberOfApprentices,
                ukprn,
                providerName,
                providerEmail,
                providerPhone,
                providerWebsite,
                false);

            email.Tokens["FatURL"].Should().Be("---");
        }
    }
}
