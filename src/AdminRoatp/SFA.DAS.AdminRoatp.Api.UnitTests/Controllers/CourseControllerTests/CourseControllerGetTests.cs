using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllowedProviders;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.CourseControllerTests;

public class CourseControllerGetTests
{
    [Test, MoqAutoData]
    public async Task WhenGetAllowedProvidersByCourseIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CourseController sut,
        GetAllowedProvidersResponse expected,
        GetAllowedProvidersQuery query)
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllowedProvidersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await sut.GetAllowedProvidersByCourse(query.larsCode);

        // Assert
        result.As<OkObjectResult>().Value.Should().Be(expected);
    }
}
