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

using SFA.DAS.Campaign.Application.Queries.PreviewLandingPage;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.LandingPage
{
    public class WhenGettingPreviewContent
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Landing_Page_Is_Returned_From_The_Mediator_Query(
            string hubName,
            string slugName,
            GetPreviewLandingPageQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LandingPageController controller)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPreviewLandingPageQuery>(c => c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.GetPreviewLandingPage(hubName, slugName) as OkObjectResult;
         
            Assert.IsNotNull(actual);
            var actualResult = actual.Value as GetLandingPageResponse;
            Assert.IsNotNull(actualResult);
            actualResult.LandingPage.Should().BeEquivalentTo(mediatorResult.PageModel);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_NotFound_Is_Returned_If_No_Result(
            string hubName,
            string slugName,
            GetPreviewLandingPageQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LandingPageController controller)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPreviewLandingPageQuery>(c => c.Hub.Equals(hubName) && c.Slug.Equals(slugName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPreviewLandingPageQueryResult
                {
                    PageModel = null
                });

            var actual = await controller.GetPreviewLandingPage(hubName, slugName) as NotFoundObjectResult;
         
            Assert.IsNotNull(actual);
            var actualResult = actual.Value as NotFoundResponse;
            Assert.IsNotNull(actualResult);
            actualResult.Message.Should().Be($"Preview landing page not found for {hubName}/{slugName}");
        }
    }
}