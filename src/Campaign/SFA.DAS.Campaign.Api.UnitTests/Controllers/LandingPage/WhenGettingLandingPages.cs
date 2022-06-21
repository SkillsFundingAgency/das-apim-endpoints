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

using SFA.DAS.Campaign.Application.Queries.LandingPage;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.LandingPage
{
    public class WhenGettingLandingPages
    {

        [Test, RecursiveMoqAutoData]
        public async Task And_Given_A_Valid_Hub_And_A_Slug_Then_The_Landing_Page_Is_Returned(
            string hubName,
            string slugName,
            GetLandingPageQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LandingPageController controller)
        {
            SetupMediator(mediatorResult, mockMediator, hubName, slugName);

            var controllerResult = await InstantiateController<OkObjectResult>(controller, hubName, slugName);
         
            var actualResult = controllerResult.Value as GetLandingPageResponse;
            Assert.IsNotNull(actualResult);
            actualResult.LandingPage.Should().BeEquivalentTo(mediatorResult.PageModel);
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Given_Invalid_Hub_And_A_Slug_Then_The_Landing_Page_Is_Not_Returned(
            string hubName,
            string slugName,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LandingPageController controller)
        {
            SetupMediator(new GetLandingPageQueryResult(), mockMediator, hubName, slugName);

            var controllerResult = await InstantiateController<NotFoundObjectResult>(controller, hubName, slugName);

            var actualResult = controllerResult.Value as NotFoundResponse;
            Assert.IsNotNull(actualResult);
            actualResult.Message.Should().Be($"Landing page not found for {hubName}/{slugName}");
        }

        private static async Task<T> InstantiateController<T>(LandingPageController controller, string hubName, string slugName)
        {
            var controllerResult = (T) await controller.GetLandingPageAsync(hubName, slugName, CancellationToken.None);

            return controllerResult;
        }

        private static void SetupMediator(GetLandingPageQueryResult mediatorResult, Mock<IMediator> mockMediator, string hubName, string slugName)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetLandingPageQuery>(c=>c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
        }
    }
}
