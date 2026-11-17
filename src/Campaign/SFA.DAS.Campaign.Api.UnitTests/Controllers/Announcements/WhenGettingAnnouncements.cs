using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Controllers;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Announcements;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Announcements
{
    public class WhenGettingAnnouncements
    {
        [Test, MoqAutoData]
        public async Task Then_The_Announcements_Are_Returned(
            GetAnnouncementsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AnnouncementsController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetAnnouncementsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAnnouncementsAsync(CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as GetAnnouncementsResponse;

            controllerResult.StatusCode.Should().Be(200);
            actual.Announcements.Should().BeEquivalentTo(mediatorResult.PageModel);
        }
    }
}
