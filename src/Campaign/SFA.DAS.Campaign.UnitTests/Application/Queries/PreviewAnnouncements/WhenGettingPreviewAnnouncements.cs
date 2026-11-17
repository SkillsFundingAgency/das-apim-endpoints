using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.PreviewAnnouncements;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.PreviewAnnouncements
{
    public class WhenGettingPreviewAnnouncements
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Preview_Api_Is_Called_And_The_Announcements_Are_Returned(
            GetPreviewAnnouncementsQuery query,
            CmsContent apiResponse,
            [Frozen] Mock<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>> apiClient,
            GetPreviewAnnouncementsQueryHandler handler)
        {
            apiClient.Setup(o => o.Get<CmsContent>(It.Is<GetAnnouncementsRequest>(c =>
                    c.GetUrl.Contains("entries?content_type=announcements") && c.GetUrl.Contains("order=-fields.date"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(new AnnouncementsPageModel().Build(apiResponse));
        }
    }
}
