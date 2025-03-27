using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeshipsManage.Api.Controllers;
using SFA.DAS.ApprenticeshipsManage.Api.Models;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeshipsManage.Api.Tests.Controllers;
public class ApprenticeshipsControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
          string ukprn,
          int page,
          int pageSize,
          GetApprenticeshipsQueryResult getApprenticeshipsResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] ApprenticeshipsController appretniceshipsController)
    {
        string academicyear = "2024-09-01";

        var validDate = DateTime.TryParse(academicyear, out var searchDateValue);

        mockMediator
            .Setup(x => x.Send(
                It.Is<GetApprenticeshipsQuery>(x =>
                x.Ukprn == ukprn &&
                x.AcademicYearDate == searchDateValue &&
                x.Page == page &&
                x.PageSize == Math.Clamp(pageSize, 10, 100)
                ),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(getApprenticeshipsResult);

        var controllerResult = await appretniceshipsController.GetApprenticeships(ukprn, academicyear, page, pageSize) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var model = controllerResult.Value as GetApprenticeshipsResponse;
        model.Should().NotBeNull();
        model!.Apprenticeships.Should().BeEquivalentTo(getApprenticeshipsResult.Items);
        model.Page.Should().Be(getApprenticeshipsResult.Page);
        model.PageSize.Should().Be(getApprenticeshipsResult.PageSize);
        model.Total.Should().Be(getApprenticeshipsResult.TotalItems);
        model.TotalPages.Should().Be(getApprenticeshipsResult.TotalPages);
    }

    [Test, MoqAutoData]
    public async Task Then_Invalid_Date_Returns_BadRequest(
         string ukprn,
         int page,
         int pageSize,
         [Greedy] ApprenticeshipsController appretniceshipsController)
    {
        string academicyear = "2425";


        var controllerResult = await appretniceshipsController.GetApprenticeships(ukprn, academicyear, page, pageSize) as BadRequestResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
