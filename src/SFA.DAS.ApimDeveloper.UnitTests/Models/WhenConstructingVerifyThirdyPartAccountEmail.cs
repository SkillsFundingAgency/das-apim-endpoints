using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Models;

namespace SFA.DAS.ApimDeveloper.UnitTests.Models
{
    public class WhenConstructingVerifyThirdyPartAccountEmail
    {
        [Test, AutoData]
        public void Then_Values_Are_Set_Correctly(
            CreateUserCommand command, ApimDeveloperMessagingConfiguration config)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"Contact", $"{command.FirstName} {command.LastName}" },
                {"ConfirmEmailURL", command.ConfirmationEmailLink }
            };

            var email = new VerifyThirdyPartyAccountEmail(command, config);

            email.TemplateId.Should().Be(config.VerifyThirdPartyAccountTemplateId);
            email.RecipientAddress.Should().Be(command.Email);
            email.ReplyToAddress.Should().Be(EmailConstants.ReplyToAddress);
            email.Tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}