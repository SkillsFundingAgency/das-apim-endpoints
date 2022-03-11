using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ChangePassword;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Models;

namespace SFA.DAS.ApimDeveloper.UnitTests.Models
{
    public class WhenConstructingChangePasswordEmail
    {
        [Test, AutoData]
        public void Then_Values_Are_Set_Correctly(
            SendEmailToChangePasswordCommand command, ApimDeveloperMessagingConfiguration config)
        {
            var expectedTokens = new Dictionary<string, string>
            {
                {"Contact", $"{command.FirstName} {command.LastName}" },
                {"ChangePasswordURL", command.ChangePasswordUrl }
            };

            var email = new ChangePasswordEmail(command, config);

            email.TemplateId.Should().Be(config.ChangePasswordTemplateId);
            email.RecipientAddress.Should().Be(command.Email);
            email.ReplyToAddress.Should().Be(EmailConstants.ReplyToAddress);
            email.Tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}