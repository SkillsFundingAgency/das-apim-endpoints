using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.Schools.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;
public class SchoolsControllerTests
{
    [Test, MoqAutoData]
    public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
        GetSchoolsQueryApiResult response,
        [Frozen] Mock<IMediator> mockMediator,
        string searchTerm)
    {
        mockMediator.Setup(m => m.Send(It.Is<GetSchoolsQuery>(x => x.SearchTerm == searchTerm), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new SchoolsController(mockMediator.Object);

        var getSchoolResponse = await controller.GetSchools(searchTerm);

        (((GetSchoolsQueryResult)getSchoolResponse.As<OkObjectResult>().Value!)!).Schools.Should()
                .BeEquivalentTo(response.Data);

        mockMediator.Verify(m =>
            m.Send(It.Is<GetSchoolsQuery>(q => q.SearchTerm == searchTerm), It.IsAny<CancellationToken>()));
    }
}
