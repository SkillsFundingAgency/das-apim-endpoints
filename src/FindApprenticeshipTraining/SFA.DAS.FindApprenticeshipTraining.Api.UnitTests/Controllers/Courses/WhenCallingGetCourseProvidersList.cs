using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses
{
    public class WhenCallingGetCourseProvidersList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_Pagination_Details_And_Providers_From_Mediator(
            int id,
            GetCourseProvidersRequest request,
            GetProvidersListFromCourseIdResponse mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] ILogger<CoursesController> mockLogger,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c =>
                        c.Id.Equals(id)
                        && c.OrderBy.Equals(request.OrderBy)
                        && c.Distance.Equals(request.Distance)
                        && c.Latitude.Equals(request.Latitude)
                        && c.Longitude.Equals(request.Longitude)
                        && c.Longitude.Equals(request.Longitude)
                        && c.DeliveryModes.Equals(request.DeliveryModes)
                        && c.EmployerProviderRatings.Equals(request.EmployerProviderRatings)
                        && c.ApprenticeProviderRatings.Equals(request.ApprenticeProviderRatings)
                        && c.Qar.Equals(request.Qar)
                        && c.Page.Equals(request.Page)
                        && c.PageSize.Equals(request.PageSize)
                        && c.ShortlistUserId.Equals(request.ShortlistUserId)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = controllerResult.Value as GetProvidersListFromCourseIdResponse;
            Assert.That(model, Is.Not.Null);

            model.Should().Be(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int id,
            GetCourseProvidersRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviders(id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test, MoqAutoData]
        public async Task And_Null_Result_Then_Returns_Bad_Request(
            int id,
            GetCourseProvidersRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetProvidersListFromCourseIdResponse)null);

            var controllerResult = await controller.GetProviders(id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
