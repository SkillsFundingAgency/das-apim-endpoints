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
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemandsOlderThan3Years;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenGettingCourseDemandsOlderThan3Years
    {
        [Test, MoqAutoData]
        public async Task Then_The_Demands_Are_Returned_From_Mediator(
            GetCourseDemandsOlderThan3YearsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            //Arrange
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetCourseDemandsOlderThan3YearsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);
            
            //Act
            var actual = await controller.CourseDemandsOlderThan3Years() as ObjectResult;
            
            //Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int) HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(new{queryResult.EmployerDemandIds});
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DemandController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetCourseDemandsOlderThan3YearsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());
            
            //Act
            var actual = await controller.CourseDemandsOlderThan3Years() as StatusCodeResult;
            
            //Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}