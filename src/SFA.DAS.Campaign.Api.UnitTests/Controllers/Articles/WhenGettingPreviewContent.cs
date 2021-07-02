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
using SFA.DAS.Campaign.Application.Queries.PreviewArticles;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Articles
{
    public class WhenGettingPreviewContent
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Article_Is_Returned_From_The_Mediator_Query(
            string hubName,
            string slugName,
            GetPreviewArticleByHubAndSlugQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ArticleController controller)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPreviewArticleByHubAndSlugQuery>(c => c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.GetPreviewArticle(hubName, slugName) as OkObjectResult;
         
            Assert.IsNotNull(actual);
            var actualResult = actual.Value as GetArticleResponse;
            Assert.IsNotNull(actualResult);
            actualResult.Article.Should().BeEquivalentTo(mediatorResult.PageModel);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_NotFound_Is_Returned_If_No_Result(
            string hubName,
            string slugName,
            GetPreviewArticleByHubAndSlugQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ArticleController controller)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPreviewArticleByHubAndSlugQuery>(c => c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPreviewArticleByHubAndSlugQueryResult
                {
                    PageModel = null
                });

            var actual = await controller.GetPreviewArticle(hubName, slugName) as NotFoundObjectResult;
         
            Assert.IsNotNull(actual);
            var actualResult = actual.Value as NotFoundResponse;
            Assert.IsNotNull(actualResult);
            actualResult.Message.Should().Be($"Preview article not found for {hubName}/{slugName}");
        }
    }
}