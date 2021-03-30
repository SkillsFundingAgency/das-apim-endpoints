using System;
using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.FindApprenticeshipTraining.Application;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCoursesList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_And_Sectors_And_Levels_From_Mediator_And_Shortlist_Count(
            Guid? shortlistUserId,
            GetTrainingCoursesListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCoursesListQuery>(c=>c.ShortlistUserId.Equals(shortlistUserId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(shortlistUserId:shortlistUserId) as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCoursesListResponse;
            model.TrainingCourses.Should().BeEquivalentTo(mediatorResult.Courses.Select(item => (GetTrainingCourseListItem)item));
            model.Sectors.Should().BeEquivalentTo(mediatorResult.Sectors, options=>options.Excluding(c=>c.Name));
            model.Levels.Should().BeEquivalentTo(mediatorResult.Levels);
            model.ShortlistItemCount.Should().Be(mediatorResult.ShortlistItemCount);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_From_Mediator_With_Keyword_And_RouteIds_And_Levels_And_OrderBy_If_Supplied(
            string keyword,
            List<int> levels,
            List<string> routeIds,
            GetTrainingCoursesListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCoursesListQuery>(c=>
                                                            c.Keyword.Equals(keyword) 
                                                            && c.RouteIds.Equals(routeIds)
                                                            && c.Levels.Equals(levels)
                                                            && c.OrderBy.Equals(OrderBy.Score)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(keyword, routeIds, levels, "Relevance") as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCoursesListResponse;
            model.TrainingCourses.Should().BeEquivalentTo(mediatorResult.Courses.Select(item => (GetTrainingCourseListItem)item));
            model.Total.Should().Be(mediatorResult.Total);
            model.TotalFiltered.Should().Be(mediatorResult.TotalFiltered);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCoursesListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
