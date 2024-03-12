using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Providers_Count_From_Mediator(
            int standardCode,
            double lat,
            double lon,
            string locationName,
            Guid? shortlistUserId,
            GetTrainingCourseResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseQuery>(c=>
                        c.Id.Equals(standardCode)
                        && c.Lat.Equals(lat)
                        && c.Lon.Equals(lon)
                        && c.LocationName.Equals(locationName)
                        && c.ShortlistUserId.Equals(shortlistUserId)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(standardCode, lat, lon, locationName, shortlistUserId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseResponse;
            Assert.That(model, Is.Not.Null);
            model.TrainingCourse.Should().BeEquivalentTo((GetTrainingCourseListItem)mediatorResult.Course);

            model.TrainingCourse.Id.Should().Be(mediatorResult.Course.LarsCode);
            model.ProvidersCount.TotalProviders.Should().Be(mediatorResult.ProvidersCount);
            model.ProvidersCount.ProvidersAtLocation.Should().Be(mediatorResult.ProvidersCountAtLocation);
            model.ShortlistItemCount.Should().Be(mediatorResult.ShortlistItemCount);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Response_Then_Not_Found_Returned(
            int standardCode,
            GetTrainingCourseResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseQuery>(c=>
                        c.Id.Equals(standardCode)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTrainingCourseResult());

            var controllerResult = await controller.Get(standardCode) as StatusCodeResult;
            
            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int standardCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(standardCode) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}