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
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenStartingCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Demand_Is_Returned_From_Mediator(
            int id,
            GetStartCourseDemandQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DemandController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetStartCourseDemandQuery>(c => c.CourseId.Equals(id)), CancellationToken.None))
                .ReturnsAsync(queryResult);
            
            //Act
            var actual = await controller.StartDemand(id) as ObjectResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var actualModel = actual.Value as GetStartCourseDemandResponse;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(queryResult.Course, options=>options
                .Excluding(c=>c.LarsCode)
                .Excluding(c=>c.StandardUId)
                .Excluding(c => c.StandardDates)
            );
            actualModel.TrainingCourse.LastStartDate.Should().Be(queryResult.Course.StandardDates.LastDateStarts);
        }
        [Test, MoqAutoData]
        public async Task Then_If_Exception_Returned_Then_Internal_Error_Response_Returned(
            int id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DemandController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetStartCourseDemandQuery>(c => c.CourseId.Equals(id)), CancellationToken.None))
                .ThrowsAsync(new Exception());
            
            //Act
            var actual = await controller.StartDemand(id) as StatusCodeResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}