using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses
{
    public class WhenCallingGetCourseProvidersList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_Providers_From_Mediator(
            int idVal,
            GetCourseProvidersModel getCourseProvidersModel,
            GetCourseProvidersResponse mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] ILogger<CoursesController> mockLogger,
            [Greedy] CoursesController controller)
        {
            var id = idVal.ToString();

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseProvidersQuery>(c =>
                        c.Id.Equals(id)
                        && c.OrderBy.Equals(getCourseProvidersModel.OrderBy)
                        && c.Distance.Equals(getCourseProvidersModel.Distance)
                        && c.Location.Equals(getCourseProvidersModel.Location)
                        && c.DeliveryModes.Equals(getCourseProvidersModel.DeliveryModes)
                        && c.EmployerProviderRatings.Equals(getCourseProvidersModel.EmployerProviderRatings)
                        && c.ApprenticeProviderRatings.Equals(getCourseProvidersModel.ApprenticeProviderRatings)
                        && c.Qar.Equals(getCourseProvidersModel.Qar)
                        && c.Page.Equals(getCourseProvidersModel.Page)
                        && c.PageSize.Equals(getCourseProvidersModel.PageSize)
                        && c.ShortlistUserId.Equals(getCourseProvidersModel.ShortlistUserId)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetCourseProviders(id, getCourseProvidersModel) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = controllerResult.Value as GetCourseProvidersResponse;
            Assert.That(model, Is.Not.Null);

            model.Should().Be(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Empty_Response_Then_Returns_Empty_Result(
            int idVal,
            GetCourseProvidersModel getCourseProvidersModel,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            string id = idVal.ToString();
            var response = new GetCourseProvidersResponse();
            mockMediator
                 .Setup(mediator => mediator.Send(
                     It.IsAny<GetCourseProvidersQuery>(),
                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

            var controllerResult = await controller.GetCourseProviders(id, getCourseProvidersModel) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = controllerResult.Value as GetCourseProvidersResponse;
            Assert.That(model, Is.Not.Null);

            model.Should().BeEquivalentTo(new GetCourseProvidersResponse());
        }

        [Test, MoqAutoData]
        public async Task And_Null_Response_Returns_NotFound(
            string id,
            GetCourseProvidersModel getCourseProvidersModel,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller
        )
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetCourseProvidersResponse)null);

            var controllerResult = await controller.GetCourseProviders(id, getCourseProvidersModel) as NotFoundResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
