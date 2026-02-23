using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Commands.Cmad;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetCommitmentsApprenticeshipById;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByAccountDetails;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRevisionById;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Controllers
{
    public class CmadControllerTests
    {
        [Test, MoqAutoData]
        public async Task GetRegistrationsByAccountDetails_Returns_Ok_With_Registrations(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] CmadController controller,
            GetRegistrationsByAccountDetailsQuery query,
            List<Registration> registrations)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var expectedResult = new GetRegistrationsByAccountDetailsQueryResult { Registrations = registrations };

            // Ensure deterministic values on query for verification
            query.FirstName = "Alice";
            query.LastName = "Smith";
            query.DateOfBirth = new DateTime(1990, 2, 3);

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetRegistrationsByAccountDetailsQuery>(q =>
                        q.FirstName == query.FirstName &&
                        q.LastName == query.LastName &&
                        q.DateOfBirth == query.DateOfBirth),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetRegistrationsByAccountDetails(query.FirstName, query.LastName, query.DateOfBirth);

            // Assert
            var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(registrations);

            mediatorMock.Verify(m => m.Send(
                    It.Is<GetRegistrationsByAccountDetailsQuery>(q =>
                        q.FirstName == query.FirstName &&
                        q.LastName == query.LastName &&
                        q.DateOfBirth == query.DateOfBirth),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRevisionsById_Returns_Ok_With_Revision(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] CmadController controller,
            GetRevisionsByIdQuery query,
            Revision revision)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            query.ApprenticeId = Guid.NewGuid();
            query.ApprenticeshipId = 1234L;
            query.RevisionId = 5678L;

            var expectedResult = new GetRevisionsByIdQueryResult { Revision = revision };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetRevisionsByIdQuery>(q =>
                        q.ApprenticeId == query.ApprenticeId &&
                        q.ApprenticeshipId == query.ApprenticeshipId &&
                        q.RevisionId == query.RevisionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetRevisionsById(query.ApprenticeId, query.ApprenticeshipId, query.RevisionId);

            // Assert
            var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(revision);

            mediatorMock.Verify(m => m.Send(
                    It.Is<GetRevisionsByIdQuery>(q =>
                        q.ApprenticeId == query.ApprenticeId &&
                        q.ApprenticeshipId == query.ApprenticeshipId &&
                        q.RevisionId == query.RevisionId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetCommitmentsApprenticeshipById_Returns_Ok_With_Response(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] CmadController controller,
            long apprenticeshipId,
            GetApprenticeshipResponse expectedResponse)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetCommitmentsApprenticeshipByIdQuery>(q => q.ApprenticeshipId == apprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await controller.GetCommitmentsApprenticeshipById(apprenticeshipId);

            // Assert
            var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(expectedResponse);

            mediatorMock.Verify(m => m.Send(
                    It.Is<GetCommitmentsApprenticeshipByIdQuery>(q => q.ApprenticeshipId == apprenticeshipId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task CreateApprenticeship_Returns_Ok_And_Sends_Command(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] CmadController controller,
            Guid registrationId,
            Guid apprenticeId,
            string lastName,
            DateTime dateOfBirth)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<CreateApprenticeshipFromRegistrationCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(MediatR.Unit.Value);

            // Act
            var actionResult = await controller.CreateApprenticeship(registrationId, apprenticeId, lastName, dateOfBirth);

            // Assert
            actionResult.Should().BeOfType<OkResult>();

            mediatorMock.Verify(m => m.Send(
                    It.Is<CreateApprenticeshipFromRegistrationCommand>(c =>
                        c.RegistrationId == registrationId &&
                        c.ApprenticeId == apprenticeId &&
                        c.LastName == lastName &&
                        c.DateOfBirth == dateOfBirth),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }



        [Test, MoqAutoData]
        public async Task CreateMyApprenticeship_Returns_StatusCode_And_Sends_Command_When_Success(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CmadController controller,
        Guid apprenticeId,
        CreateMyApprenticeshipData data)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.OK, null);

            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<CreateMyApprenticeshipCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actionResult = await controller.CreateMyApprenticeship(apprenticeId, data);

            // Assert
            actionResult.Should().BeOfType<StatusCodeResult>();
            var statusCodeResult = (StatusCodeResult)actionResult;
            statusCodeResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            mediatorMock.Verify(m => m.Send(
                    It.Is<CreateMyApprenticeshipCommand>(c =>
                        c.ApprenticeId == apprenticeId &&
                        c.Data == data),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task CreateMyApprenticeship_Returns_StatusCode_With_ErrorContent_When_Failure(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] CmadController controller,
            Guid apprenticeId,
            CreateMyApprenticeshipData data)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.BadRequest, "Something went wrong");

            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<CreateMyApprenticeshipCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actionResult = await controller.CreateMyApprenticeship(apprenticeId, data);

            // Assert - when there's ErrorContent the controller returns StatusCode(int, object) which produces an ObjectResult
            actionResult.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)actionResult;
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            objectResult.Value.Should().Be("Something went wrong");

            mediatorMock.Verify(m => m.Send(
                    It.Is<CreateMyApprenticeshipCommand>(c =>
                        c.ApprenticeId == apprenticeId &&
                        c.Data == data),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}