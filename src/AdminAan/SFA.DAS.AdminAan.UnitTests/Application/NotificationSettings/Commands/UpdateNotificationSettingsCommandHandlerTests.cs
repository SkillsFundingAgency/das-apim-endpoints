using AutoFixture.NUnit3;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SFA.DAS.AdminAan.Application.NotificationsSettings.Commands;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.NotificationSettings.Commands
{
    public class UpdateNotificationSettingsCommandHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_Updates_NotificationPreferences(
            UpdateNotificationSettingsCommand command,
            [Frozen] Mock<IAanHubRestApiClient> apiClient,
            [Frozen] UpdateNotificationSettingsCommandHandler handler)
        {
            apiClient.Setup(x =>
                x.UpdateMember(
                    It.IsAny<Guid>(),
                It.IsAny<JsonPatchDocument<PatchMemberModel>>(),
                    It.IsAny<CancellationToken>()));

            await handler.Handle(command, CancellationToken.None);

            var expectedPatchDocument = new JsonPatchDocument<PatchMemberModel>();
            expectedPatchDocument.Replace(model => model.ReceiveNotifications, command.ReceiveNotifications);
            var expectedOperation = expectedPatchDocument.Operations.First();

            apiClient.Verify(x =>
                x.UpdateMember(
                    It.Is<Guid>(id => id == command.MemberId),
                    It.Is<JsonPatchDocument<PatchMemberModel>>(document =>
                        document.Operations.Count == 1
                        && document.Operations.Any(o =>
                            o.OperationType == expectedOperation.OperationType
                            && o.path == expectedOperation.path
                            && o.value.Equals(expectedOperation.value))),
                        It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
