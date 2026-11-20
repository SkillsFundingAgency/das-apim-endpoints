using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.PreviewHomepage;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.PreviewHomepage
{
    public class WhenGettingThePreviewHomepage
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Preview_Api_Is_Called_And_The_Homepage_Is_Returned(
            GetPreviewHomepageQuery query,
            CmsContent apiResponse,
            [Frozen] Mock<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>> apiClient,
            GetPreviewHomepageQueryHandler handler)
        {
            apiClient.Setup(o => o.Get<CmsContent>(It.Is<GetHomepageRequest>(c => c.GetUrl.Contains("entries?content_type=homepage"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(new HomepageModel().Build(apiResponse));
        }
    }
}
