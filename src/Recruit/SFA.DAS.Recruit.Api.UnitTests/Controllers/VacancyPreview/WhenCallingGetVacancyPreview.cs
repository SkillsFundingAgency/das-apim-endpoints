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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.VacancyPreview;

public class WhenCallingGetVacancyPreview
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Called_And_Response_Returned(
        int standardId,
        GetVacancyPreviewQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyPreviewController controller)
    {
        mediatorResult.Course.Level = 5;
        mediator.Setup(x =>
                x.Send(It.Is<GetVacancyPreviewQuery>(c => c.StandardId == standardId), CancellationToken.None))
            .ReturnsAsync(mediatorResult);
        
        var actual = await controller.GetPreview(standardId) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = (GetVacancyPreviewApiResponse)actual!.Value!;
        actualModel.Should().BeEquivalentTo((GetVacancyPreviewApiResponse)mediatorResult, options=> options.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Result_NotFound_Returned(
        int standardId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyPreviewController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetVacancyPreviewQuery>(c => c.StandardId == standardId), CancellationToken.None))
            .ReturnsAsync(new GetVacancyPreviewQueryResult());
        
        var actual = await controller.GetPreview(standardId) as NotFoundResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_InternalServer_Response_Returned(
        int standardId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyPreviewController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetVacancyPreviewQuery>(c => c.StandardId == standardId), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.GetPreview(standardId) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}