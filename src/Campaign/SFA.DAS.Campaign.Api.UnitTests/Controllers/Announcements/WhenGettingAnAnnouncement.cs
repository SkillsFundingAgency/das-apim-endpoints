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
using SFA.DAS.Campaign.Application.Queries.Announcement;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Announcements
{
    public class WhenGettingAnAnnouncement
    {
        [Test, MoqAutoData]
        public async Task Then_The_Announcement_Is_Returned(
            string slug,
            GetAnnouncementBySlugQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AnnouncementsController controller)
        {
            mockMediator.Setup(m => m.Send(It.Is<GetAnnouncementBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAnnouncementAsync(slug, CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as GetAnnouncementResponse;

            controllerResult.StatusCode.Should().Be(200);
            actual.Announcement.Should().BeEquivalentTo(mediatorResult.PageModel);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Found_A_Not_Found_Result_Is_Returned(
            string slug,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AnnouncementsController controller)
        {
            mockMediator.Setup(m => m.Send(It.Is<GetAnnouncementBySlugQuery>(q => q.Slug == slug), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAnnouncementBySlugQueryResult { PageModel = null });

            var controllerResult = await controller.GetAnnouncementAsync(slug, CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as NotFoundResponse;

            controllerResult.StatusCode.Should().Be(404);
            actual.Message.Should().Be($"Announcement not found for slug {slug}.");
        }
    }
}
