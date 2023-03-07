using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers
{
    public class ProfilesControllerTests
    {
        [Test]
        [MoqAutoData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk(GetProfilesByUserTypeQueryResult response,
            [Frozen] Mock<IMediator> mockMediator)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new ProfilesController(mockMediator.Object);

            var userType = "Apprentice";
            var result = await controller.GetProfilesByUserType(userType) as OkObjectResult;

            result.Should().NotBeNull();

            var model = result?.Value;

            model.Should().BeEquivalentTo(response);
        }

        [Test]
        [MoqAutoData]
        public async Task And_NoProfilesReturnedFromMediator_Then_Return500(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProfilesController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), CancellationToken.None))
                .ReturnsAsync((GetProfilesByUserTypeQueryResult)null!);

            var userType = "Apprentice";
            var controllerResult = await controller.GetProfilesByUserType(userType) as NotFoundResult;

            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}