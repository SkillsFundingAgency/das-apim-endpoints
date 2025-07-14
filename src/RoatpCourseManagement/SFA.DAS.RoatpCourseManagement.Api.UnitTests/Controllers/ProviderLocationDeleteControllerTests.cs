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
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.DeleteProviderLocation;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers;

[TestFixture]
public class ProviderLocationDeleteControllerTests
{

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_InvokesCommand(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, Guid userId, string userDisplayName)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.OK, "");

        DeleteProviderLocationCommand command = new DeleteProviderLocationCommand
        { Ukprn = ukprn, Id = id, UserId = userId.ToString(), UserDisplayName = userDisplayName };


        mediatorMock.Setup(m => m.Send(It.Is<DeleteProviderLocationCommand>(c => c.Ukprn == ukprn
                && c.Id == id && c.UserId == userId.ToString()
                && c.UserDisplayName == userDisplayName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        await sut.DeleteProviderLocation(ukprn, id, userId, userDisplayName);

        mediatorMock.Verify(m => m.Send(It.Is<DeleteProviderLocationCommand>(c => c.Ukprn == ukprn
            && c.Id == id && c.UserId == userId.ToString()
            && c.UserDisplayName == userDisplayName), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_ReturnsNoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, Guid userId, string userDisplayName)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.NoContent, "");
        DeleteProviderLocationCommand command = new DeleteProviderLocationCommand
        { Ukprn = ukprn, Id = id, UserId = userId.ToString(), UserDisplayName = userDisplayName };

        mediatorMock.Setup(m => m.Send(It.Is<DeleteProviderLocationCommand>(c => c.Ukprn == ukprn
                    && c.Id == id && c.UserId == userId.ToString()
                    && c.UserDisplayName == userDisplayName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var response = await sut.DeleteProviderLocation(ukprn, id, userId, userDisplayName);

        (response as NoContentResult).Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, Guid userId, string userDisplayName)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.NotFound, "");
        DeleteProviderLocationCommand command = new DeleteProviderLocationCommand
        { Ukprn = ukprn, Id = id, UserId = userId.ToString(), UserDisplayName = userDisplayName };

        mediatorMock.Setup(m => m.Send(It.Is<DeleteProviderLocationCommand>(c => c.Ukprn == ukprn
                    && c.Id == id && c.UserId == userId.ToString()
                    && c.UserDisplayName == userDisplayName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var response = await sut.DeleteProviderLocation(ukprn, id, userId, userDisplayName);

        (response as NotFoundResult).Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_ReturnsBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, Guid userId, string userDisplayName)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.BadRequest, "");
        DeleteProviderLocationCommand command = new DeleteProviderLocationCommand
        { Ukprn = ukprn, Id = id, UserId = userId.ToString(), UserDisplayName = userDisplayName };

        mediatorMock.Setup(m => m.Send(It.Is<DeleteProviderLocationCommand>(c => c.Ukprn == ukprn
                    && c.Id == id && c.UserId == userId.ToString()
                    && c.UserDisplayName == userDisplayName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var response = await sut.DeleteProviderLocation(ukprn, id, userId, userDisplayName);

        (response as BadRequestResult).Should().NotBeNull();
    }
}
