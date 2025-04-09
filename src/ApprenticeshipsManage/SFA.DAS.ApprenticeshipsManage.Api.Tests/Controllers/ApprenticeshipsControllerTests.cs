using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using SFA.DAS.ApprenticeshipsManage.Api.Controllers;
using SFA.DAS.ApprenticeshipsManage.Api.Models;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeshipsManage.Api.Tests.Controllers;
public class ApprenticeshipsControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
          string ukprn,
          int page,
          int pageSize,
          KeyValuePair<string, StringValues> pageLinks,
          GetApprenticeshipsQueryResult getApprenticeshipsResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Frozen] Mock<IPagedLinkHeaderService> pagedLinkHeaderService,
          [Greedy] ApprenticeshipsController apprenticeshipsController)
    {
        int academicyear = 2425;

        mockMediator
            .Setup(x => x.Send(
                It.Is<GetApprenticeshipsQuery>(x =>
                x.Ukprn == ukprn &&
                x.AcademicYear == academicyear &&
                x.Page == page &&
                x.PageSize == Math.Clamp(pageSize, 10, 100)
                ),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(getApprenticeshipsResult);

        pagedLinkHeaderService.Setup(x => x.GetPageLinks(It.IsAny<GetApprenticeshipsQuery>(), getApprenticeshipsResult)).Returns(pageLinks);

        var httpContext = new DefaultHttpContext();

        apprenticeshipsController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var controllerResult = await apprenticeshipsController.GetApprenticeships(ukprn, academicyear, page, pageSize) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var model = controllerResult.Value as GetApprenticeshipsResponse;
        model.Should().NotBeNull();
        model!.Apprenticeships.Should().BeEquivalentTo(getApprenticeshipsResult.Items);
        model.Page.Should().Be(getApprenticeshipsResult.Page);
        model.PageSize.Should().Be(getApprenticeshipsResult.PageSize);
        model.Total.Should().Be(getApprenticeshipsResult.TotalItems);
        model.TotalPages.Should().Be(getApprenticeshipsResult.TotalPages);

        httpContext.Response.Headers.Should().Contain(pageLinks);
    }
}
