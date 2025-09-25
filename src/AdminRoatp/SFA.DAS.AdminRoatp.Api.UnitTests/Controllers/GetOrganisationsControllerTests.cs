using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers;
public class GetOrganisationsControllerTests
{
    [Test, MoqAutoData]

    public async Task GetOrganisations(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationsController sut,
        GetOrganisationsQuery request,
        GetOrganisationsQueryResponse expectedResponse)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetOrganisations(request.SearchTerm, It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        mediatorMock.Verify(m => m.Send(It.Is<GetOrganisationsQuery>(r => r.SearchTerm == request.SearchTerm), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
