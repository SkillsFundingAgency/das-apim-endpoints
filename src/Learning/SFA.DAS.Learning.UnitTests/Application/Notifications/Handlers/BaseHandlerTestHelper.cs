using AutoFixture;
using Moq;
using SFA.DAS.Learning.Application.Notification;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Learning.UnitTests.Application.Notifications.Handlers;

internal abstract class BaseHandlerTestHelper
{
#pragma warning disable CS8618 //Disable nullable check
    private Fixture _fixture;
    protected Mock<IExtendedNotificationService> _mockExtendedNotificationService;
    protected Recipient ExpectedEmployerRecipient;
    protected Recipient ExpectedProviderRecipient;
    protected CommitmentsApprenticeshipDetails ExpectedApprenticeshipDetails;
#pragma warning restore CS8618

    protected void Reset()
    {
        _fixture = new Fixture();
        _mockExtendedNotificationService = new Mock<IExtendedNotificationService>();

        _mockExtendedNotificationService.Setup(s => s.GetCurrentPartyIds(It.IsAny<Guid>())).ReturnsAsync(_fixture.Create<GetCurrentPartyIdsResponse>);

        ExpectedEmployerRecipient = _fixture.Create<Recipient>();
        _mockExtendedNotificationService.Setup(s => s.GetEmployerRecipients(It.IsAny<long>())).ReturnsAsync(new List<Recipient> { ExpectedEmployerRecipient });

        ExpectedProviderRecipient = _fixture.Create<Recipient>();
        _mockExtendedNotificationService.Setup(s => s.GetProviderRecipients(It.IsAny<long>())).ReturnsAsync(new List<Recipient> { ExpectedProviderRecipient });

        ExpectedApprenticeshipDetails = _fixture.Create<CommitmentsApprenticeshipDetails>();
        _mockExtendedNotificationService.Setup(s => s.GetApprenticeship(It.IsAny<GetCurrentPartyIdsResponse>())).ReturnsAsync(ExpectedApprenticeshipDetails);
    }

    protected IExtendedNotificationService GetExtendedNotificationService()
    {
        return _mockExtendedNotificationService.Object;
    }

    protected void VerifyNoMessageSentToEmployer()
    {
        _mockExtendedNotificationService.Verify(s => s.Send(
            ExpectedEmployerRecipient,
            It.IsAny<string>(),
            It.IsAny<Dictionary<string, string>>()
        ), Times.Never);
    }

    protected void VerifySentToEmployer(string expectedTemplateId, Dictionary<string, string> expectedTokens)
    {
        _mockExtendedNotificationService.Verify(s => s.GetEmployerRecipients(It.IsAny<long>()), Times.Once, "Expected for send to employer for a call to be made to get employer Recipients");
        VerifySentWithCorrectValues(ExpectedEmployerRecipient, expectedTemplateId, expectedTokens);
    }

    protected void VerifySentToProvider(string expectedTemplateId, Dictionary<string, string> expectedTokens)
    {
        _mockExtendedNotificationService.Verify(s => s.GetProviderRecipients(It.IsAny<long>()), Times.Once, "Expected for send to provider for a call to be made to get provider Recipients");
        VerifySentWithCorrectValues(ExpectedProviderRecipient, expectedTemplateId, expectedTokens);
    }

    private void VerifySentWithCorrectValues(Recipient expectedRecipient, string expectedTemplateId, Dictionary<string, string> expectedTokens)
    {
        _mockExtendedNotificationService.Verify(s => s.Send(
            It.IsAny<Recipient>(),
            It.IsAny<string>(),
            It.IsAny<Dictionary<string, string>>()
        ), Times.Once, "No message sent");

        _mockExtendedNotificationService.Verify(s => s.Send(
            It.IsAny<Recipient>(),
            expectedTemplateId,
            It.IsAny<Dictionary<string, string>>()
        ), Times.Once, "TemplateId does not match");

        _mockExtendedNotificationService.Verify(s => s.Send(
            expectedRecipient,
            It.IsAny<string>(),
            It.IsAny<Dictionary<string, string>>()
        ), Times.Once, "Not Sent to expected Recipient");

        _mockExtendedNotificationService.Verify(s => s.Send(
            It.IsAny<Recipient>(),
            It.IsAny<string>(),
            It.Is<Dictionary<string, string>>(tokens =>
                tokens.Count == expectedTokens.Count &&
                tokens.All(kv => expectedTokens.ContainsKey(kv.Key) && expectedTokens[kv.Key] == kv.Value)
            )
        ), Times.Once, "Tokens do not match");
    }
}
