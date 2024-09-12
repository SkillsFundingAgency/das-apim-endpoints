using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Transaction;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPostingSubmitEmployerRequest
    {
        private Mock<IMediator> _mockMediator;
        private Mock<ILogger<EmployerRequestsController>> _mockLogger;
        private EmployerRequestsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<EmployerRequestsController>>();
            _sut = new EmployerRequestsController(_mockMediator.Object, _mockLogger.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_The_EmployerRequestId_Is_Returned(
            long accountId,
            SubmitEmployerRequestRequest submitRequest,
            SubmitEmployerRequestResponse response,
            GetLocationResult locationResult,
            GetStandardResult standardResult,
            GetSettingsResult settingsResult,
            GetEmployerProfileUserResult employerProfileUserResult)
        {
            // Arrange
            locationResult.Location.Location = new GetLocationsListItem.Coordinates
            {
                GeoPoint = [1.0, 2.0]
            };

            _mockMediator
                .Setup(x => x.Send(It.Is<GetLocationQuery>(p => p.ExactSearchTerm == submitRequest.SingleLocation), CancellationToken.None))
                .ReturnsAsync(locationResult);
            
            _mockMediator
                .Setup(x => x.Send(It.Is<GetEmployerProfileUserQuery>(p => p.UserId == submitRequest.RequestedBy), CancellationToken.None))
                .ReturnsAsync(employerProfileUserResult);
            
            _mockMediator
                .Setup(x => x.Send(It.IsAny<SubmitEmployerRequestCommand>(), CancellationToken.None))
                .ReturnsAsync(response);

            _mockMediator
                .Setup(x => x.Send(It.Is<GetStandardQuery>(p => p.StandardId == submitRequest.StandardReference), CancellationToken.None))
                .ReturnsAsync(standardResult);

            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetSettingsQuery>(), CancellationToken.None))
                .ReturnsAsync(settingsResult);

            // Act
            var actual = await _sut.SubmitEmployerRequest(accountId, submitRequest) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(response.EmployerRequestId);
        }

        [Test, MoqAutoData]
        public async Task Then_BadRequest_Is_Returned_If_SameLocation_Is_Yes_And_Location_Not_Found(
            long accountId,
            SubmitEmployerRequestRequest submitCommand)
        {
            // Arrange
            var locationResult = new GetLocationResult { Location = null };
            _mockMediator
                .Setup(x => x.Send(It.Is<GetLocationQuery>(p => p.ExactSearchTerm == submitCommand.SingleLocation), CancellationToken.None))
                .ReturnsAsync(locationResult);

            submitCommand.SameLocation = "Yes";

            // Act
            var actual = await _sut.SubmitEmployerRequest(accountId, submitCommand) as BadRequestObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().Be($"Unable to submit employer request as the specified location {submitCommand.SingleLocation} cannot be found");
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_Same_Location_Is_Yes_And_Exception_Is_Thrown_Getting_Location(
            long accountId,
            SubmitEmployerRequestRequest submitCommand)
        {
            // Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<GetLocationQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            submitCommand.SameLocation = "Yes";

            // Act
            var actual = await _sut.SubmitEmployerRequest(accountId, submitCommand) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_EmployerProfileUser_Not_Found(
            long accountId,
            SubmitEmployerRequestRequest submitCommand,
            GetLocationResult locationResult)
        {
            // Arrange
            locationResult.Location = new GetLocationsListItem
            {
                Location = new GetLocationsListItem.Coordinates
                {
                    GeoPoint = [1.0, 2.0]
                }
            };
            _mockMediator
                .Setup(x => x.Send(It.Is<GetLocationQuery>(p => p.ExactSearchTerm == submitCommand.SingleLocation), CancellationToken.None))
                .ReturnsAsync(locationResult);

            _mockMediator
                .Setup(x => x.Send(It.Is<GetEmployerProfileUserQuery>(p => p.UserId == submitCommand.RequestedBy), CancellationToken.None))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, string.Empty));

            // Act & Assert
            Func<Task> act = async () => await _sut.SubmitEmployerRequest(accountId, submitCommand);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
