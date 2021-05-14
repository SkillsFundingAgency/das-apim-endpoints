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
        public void Then_The_Values_Are_Set_Correctly(string employerName, string trainingCourse, string confirmationLink)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"AEDEmployerName", employerName},
                {"AEDApprenticeshipTrainingCourse", trainingCourse},
                {"AEDConfirmEmailURL", confirmationLink}
            };

            var actual = new CreateVerifyEmployerDemandEmail(employerName, trainingCourse, confirmationLink);

            actual.TemplateId.Should().Be(EmailConstants.VerifyContactEmailEmployerDemandTemplateId);
            actual.Subject.Should().Be("Confirm your contact email address");
            actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}