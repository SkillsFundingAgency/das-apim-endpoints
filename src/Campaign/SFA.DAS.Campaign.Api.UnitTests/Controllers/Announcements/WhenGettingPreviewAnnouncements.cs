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
using SFA.DAS.Campaign.Application.Queries.PreviewAnnouncement;
using SFA.DAS.Campaign.Application.Queries.PreviewAnnouncements;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Announcements
{
    public class WhenGettingPreviewAnnouncements
    {
        [Test, MoqAutoData]
        public async Task Then_The_Preview_Announcements_Are_Returned(
            GetPreviewAnnouncementsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AnnouncementsController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetPreviewAnnouncementsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetPreviewAnnouncementsAsync(CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as GetAnnouncementsResponse;

            controllerResult.StatusCode.Should().Be(200);
            actual.Announcements.Should().BeEquivalentTo(mediatorResult.PageModel);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Preview_Announcement_Is_Returned(
            string slug,
            GetPreviewAnnouncementBySlugQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AnnouncementsController controller)
        {
            mockMediator.Setup(m => m.Send(It.Is<GetPreviewAnnouncementBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetPreviewAnnouncementAsync(slug, CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as GetAnnouncementResponse;

            controllerResult.StatusCode.Should().Be(200);
            actual.Announcement.Should().BeEquivalentTo(mediatorResult.PageModel);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Preview_Announcement_Is_Not_Found_A_Not_Found_Result_Is_Returned(
            string slug,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AnnouncementsController controller)
        {
            mockMediator.Setup(m => m.Send(It.Is<GetPreviewAnnouncementBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPreviewAnnouncementBySlugQueryResult { PageModel = null });

            var controllerResult = await controller.GetPreviewAnnouncementAsync(slug, CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as NotFoundResponse;

            controllerResult.StatusCode.Should().Be(404);
            actual.Message.Should().Be($"Preview announcement not found for slug {slug}.");
        }
    }
}
