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
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCourseProvider
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Providers_From_Mediator(
            int standardCode,
            int providerId,
            string location,
            double lat, 
            double lon,
            GetTrainingCourseProviderResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProviderQuery>(
                        c
                            =>c.CourseId.Equals(standardCode)
                        && c.ProviderId.Equals(providerId)
                        && c.Location.Equals(location)
                        && c.Lat.Equals(lat)
                        && c.Lon.Equals(lon)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviderCourse(standardCode,providerId, location, lat, lon) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProviderResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo(mediatorResult.Course, options=>options
                .Excluding(tc=>tc.ApprenticeshipFunding)
                .Excluding(tc=>tc.StandardDates)
                .Excluding(tc => tc.Skills)
                .Excluding(tc => tc.TypicalJobTitles)
                .Excluding(tc => tc.CoreAndOptions)
                .Excluding(tc => tc.CoreDuties)
            );
            model.TrainingCourseProvider.Should()
                .BeEquivalentTo(mediatorResult.ProviderStandard, 
                    options => options
                        .Excluding(c=>c.ContactUrl)
                        .Excluding(c=>c.StandardId)
                        .Excluding(c=>c.AchievementRates)
                        .Excluding(c=>c.Ukprn)
                        .Excluding(c=>c.DeliveryTypes)
                        .Excluding(c=>c.FeedbackRatings)
                        .Excluding(c=>c.FeedbackAttributes)
                        .Excluding(c=>c.ProviderAddress)
                );
            model.AdditionalCourses.Courses.Should().BeEquivalentTo(mediatorResult.AdditionalCourses);
            model.TrainingCourse.Should().NotBeNull();
            model.ProvidersCount.ProvidersAtLocation.Should().Be(mediatorResult.TotalProvidersAtLocation);
            model.ProvidersCount.TotalProviders.Should().Be(mediatorResult.TotalProviders);
            model.Location.Location.GeoPoint.Should().BeEquivalentTo(mediatorResult.Location.GeoPoint);
            model.Location.Name.Should().Be(mediatorResult.Location.Name);
            model.TrainingCourseProvider.Feedback.Should().NotBeNull();
            model.TrainingCourseProvider.ProviderAddress.Should().BeEquivalentTo(mediatorResult.ProviderStandard.ProviderAddress);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Provider_For_That_Standard_Then_Null_Is_Returned(
            int standardCode,
            int providerId,
            string location,
            double lat, 
            double lon,
            GetTrainingCourseProviderResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mediatorResult.ProviderStandard = null;
            mediatorResult.TotalProviders = 0;
            mediatorResult.TotalProvidersAtLocation = 0;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProviderQuery>(
                        c
                            =>c.CourseId.Equals(standardCode)
                              && c.ProviderId.Equals(providerId)
                              && c.Location.Equals(location)
                              && c.Lat.Equals(lat)
                              && c.Lon.Equals(lon)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviderCourse(standardCode,providerId, location, lat, lon) as ObjectResult;
            
            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProviderResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().NotBeNull();
            model.Location.Should().NotBeNull();
            model.TrainingCourseProvider.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Course_Or_Provider_Then_Not_Found_Returned(
            int standardCode,
            int providerId,
            string location,
            double lat, 
            double lon,
            GetTrainingCourseProviderResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mediatorResult.ProviderStandard = null;
            mediatorResult.Course = null;
            mediatorResult.TotalProviders = 0;
            mediatorResult.TotalProvidersAtLocation = 0;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProviderQuery>(
                        c
                            =>c.CourseId.Equals(standardCode)
                              && c.ProviderId.Equals(providerId)
                              && c.Location.Equals(location)
                              && c.Lat.Equals(lat)
                              && c.Lon.Equals(lon)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviderCourse(standardCode,providerId, location, lat, lon) as StatusCodeResult;
            
            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        
        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int standardCode,
            int providerId,
            string location,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviderCourse(standardCode,providerId, location) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}