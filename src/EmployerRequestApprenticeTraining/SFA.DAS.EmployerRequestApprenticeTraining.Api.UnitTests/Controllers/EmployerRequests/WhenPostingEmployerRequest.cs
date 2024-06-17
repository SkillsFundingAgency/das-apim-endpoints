using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPostingEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_EmployerRequestId_Is_Returned(
            SubmitEmployerRequestCommand submitCommand,
            CreateEmployerRequestResponse response,
            GetLocationQueryResult locationResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            locationResult.Location.Location = new GetLocationsListItem.Coordinates
            {
                GeoPoint = [1.0, 2.0]
            };
            mockMediator
                .Setup(x => x.Send(It.Is<GetLocationQuery>(p => p.ExactMatch == submitCommand.SingleLocation), CancellationToken.None))
                .ReturnsAsync(locationResult);
            mockMediator
                .Setup(x => x.Send(It.IsAny<CreateEmployerRequestCommand>(), CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            var actual = await controller.SubmitEmployerRequest(submitCommand) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(response.EmployerRequestId);
        }

        [Test, MoqAutoData]
        public async Task Then_BadRequest_Is_Returned_If_Location_Not_Found(
            SubmitEmployerRequestCommand submitCommand,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            var locationResult = new GetLocationQueryResult { Location = null };
            mockMediator
                .Setup(x => x.Send(It.Is<GetLocationQuery>(p => p.ExactMatch == submitCommand.SingleLocation), CancellationToken.None))
                .ReturnsAsync(locationResult);

            // Act
            var actual = await controller.SubmitEmployerRequest(submitCommand) as BadRequestObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().Be($"Unable to submit employer request as the specified location {submitCommand.SingleLocation} cannot be found");
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            SubmitEmployerRequestCommand submitCommand,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mockMediator.Setup(x => x.Send(It.IsAny<GetLocationQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.SubmitEmployerRequest(submitCommand) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}