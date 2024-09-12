using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.NotificationsSettings.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers
{
    public class NotificationSettingsControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_Returns_Expected_Result(
            GetNotificationsSettingsQueryResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] NotificationSettingsController sut,
            Guid memberId)
        {
            mockMediator
                .Setup(m => m.Send(It.Is<GetNotificationsSettingsQuery>(q => q.MemberId == memberId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            await sut.Index(memberId);

            result.Should().Be(result);
        }
    }
}
