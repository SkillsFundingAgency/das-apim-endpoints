using System.Net;
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
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Articles
{
    public class WhenGettingArticles
    {
        private const string HubName = "hub";
        private const string SlugName = "slug";

        [Test, RecursiveMoqAutoData]
        public async Task AndGivenAValidHubAndASlugThenTheArticleIsReturned(
            GetArticleByHubAndSlugQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ArticleController controller)
        {
            SetupMediator(mediatorResult, mockMediator);

            var controllerResult = await InstantiateController<ObjectResult>(controller);
         
            controllerResult.AssertThatTheObjectResultIsValid();
            controllerResult.AssertThatTheObjectValueIsValid<GetArticleResponse>();
        }

        [Test, RecursiveMoqAutoData]
        public async Task AndGivenInvalidHubAndASlugThenTheArticleIsNotReturned(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ArticleController controller)
        {
            SetupMediator(new GetArticleByHubAndSlugQueryResult(), mockMediator);

            var controllerResult = await InstantiateController<NotFoundObjectResult>(controller);

            controllerResult.AssertThatTheNotFoundObjectResultIsValid();
            controllerResult.AssertThatTheNotFoundObjectResultValueIsValid<NotFoundResponse>();
        }

        private static async Task<T> InstantiateController<T>(ArticleController controller)
        {
            var controllerResult = (T) await controller.GetArticleAsync(HubName, SlugName, CancellationToken.None);

            return controllerResult;
        }

        private static void SetupMediator(GetArticleByHubAndSlugQueryResult mediatorResult, Mock<IMediator> mockMediator)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.IsAny<GetArticleByHubAndSlugQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
        }
    }
}
