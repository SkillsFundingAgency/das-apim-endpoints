using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers;
public class ProvidersAllowedListControllerTests
{
    [Test, MoqAutoData]
    public async Task GetAllowedList_ReturnSuccessfulResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersAllowedListController sut,
        GetProvidersAllowedListQuery request,
        GetProvidersAllowedListQueryResponse expectedResponse)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetProvidersAllowedListQuery>(r => r.sortColumn == request.sortColumn && r.sortOrder == request.sortOrder), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetAllowedList(request.sortColumn, request.sortOrder, It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        mediatorMock.Verify(m => m.Send(It.Is<GetProvidersAllowedListQuery>(r => r.sortColumn == request.sortColumn && r.sortOrder == request.sortOrder), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
