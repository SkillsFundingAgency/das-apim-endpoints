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
        int ukprn, Guid id, DeleteProviderLocationCommand command)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.OK, "");

        mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        await sut.DeleteProviderLocation(ukprn, id, command);

        mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_ReturnsNoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, DeleteProviderLocationCommand command)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.NoContent, "");

        mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        var response = await sut.DeleteProviderLocation(ukprn, id, command);

        (response as NoContentResult).Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, DeleteProviderLocationCommand command)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.NotFound, "");

        mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        var response = await sut.DeleteProviderLocation(ukprn, id, command);

        (response as NotFoundResult).Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task DeleteProviderLocation_ReturnsBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderLocationDeleteController sut,
        int ukprn, Guid id, DeleteProviderLocationCommand command)
    {
        var apiResponse = new ApiResponse<Unit>(new Unit(), HttpStatusCode.BadRequest, "");

        mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        var response = await sut.DeleteProviderLocation(ukprn, id, command);

        (response as BadRequestResult).Should().NotBeNull();
    }
}
