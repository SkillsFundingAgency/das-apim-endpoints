using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ShortCourseTypesControllerTests;
public class ShortCourseTypesControllerGetAllShortCourseTypesTests
{
    [Test, MoqAutoData]

    public async Task GetAllShortCourseTypes_ReturnSuccessfulResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ShortCourseTypesController sut,
        GetAllShortCourseTypesQuery query,
        GetAllCourseTypesResponse expectedResponse
        )
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllShortCourseTypesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        // Act
        var result = await sut.GetAllShortCourseTypes(It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        // Assert
        mediatorMock.Verify(m => m.Send(It.IsAny<GetAllShortCourseTypesQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
}