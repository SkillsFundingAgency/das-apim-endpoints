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
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Application.Queries.LandingPage;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Articles
{
    public class WhenGettingArticles
    {

        [Test, RecursiveMoqAutoData]
        public async Task And_Given_A_Valid_Hub_And_A_Slug_Then_The_Article_Is_Returned(
            string hubName,
            string slugName,
            GetArticleByHubAndSlugQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ArticleController controller)
        {
            SetupMediator(mediatorResult, mockMediator, hubName, slugName);

            var controllerResult = await InstantiateController<OkObjectResult>(controller, hubName, slugName);
         
            var actualResult = controllerResult.Value as GetArticleResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Article.Should().BeEquivalentTo(mediatorResult.PageModel);
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Given_Invalid_Hub_And_A_Slug_Then_The_Article_Is_Not_Returned(
            string hubName,
            string slugName,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ArticleController controller)
        {
            SetupMediator(new GetArticleByHubAndSlugQueryResult(), mockMediator, hubName, slugName);

            var controllerResult = await InstantiateController<NotFoundObjectResult>(controller, hubName, slugName);

            var actualResult = controllerResult.Value as NotFoundResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Message.Should().Be($"Article not found for {hubName}/{slugName}");
        }

        private static async Task<T> InstantiateController<T>(ArticleController controller, string hubName, string slugName)
        {
            var controllerResult = (T) await controller.GetArticleAsync(hubName, slugName, CancellationToken.None);

            return controllerResult;
        }

        private static void SetupMediator(GetArticleByHubAndSlugQueryResult mediatorResult, Mock<IMediator> mockMediator, string hubName, string slugName)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetArticleByHubAndSlugQuery>(c=>c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
        }

        private static void SetupLandingPageMediator(GetLandingPageQueryResult mediatorResult, Mock<IMediator> mockMediator, string hubName, string slugName)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetLandingPageQuery>(c => c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
        }
    }
}
