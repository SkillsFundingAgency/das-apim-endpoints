using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers;

public class SectorSubjectAreaControllerTests
{
    [Test, MoqAutoData]
    public async Task GetAllSsa1_ReturnsOkWithResult(
        GetAllSectorSubjectAreaTier1Response expected,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] SectorSubjectAreaController sut)
    {
        ApiResponse<GetAllSectorSubjectAreaTier1Response> apiResponse = new(expected, System.Net.HttpStatusCode.OK, null);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSectorSubjectAreaTier1Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        var result = await sut.GetAllSectorSubjectAreaTier1();

        result.As<OkObjectResult>().Value.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task GetAllSsa1_ForwardsInnerApiResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] SectorSubjectAreaController sut)
    {
        ApiResponse<GetAllSectorSubjectAreaTier1Response> apiResponse = new(null, HttpStatusCode.BadRequest, null);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSectorSubjectAreaTier1Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        var result = await sut.GetAllSectorSubjectAreaTier1();

        result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
