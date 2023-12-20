using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.Application.Cohorts.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.BulkUpload
{
    public class WhenAddingBulkUploadDraftApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_Adding_Apprentices_From_Mediator(
                   BulkUploadAddDraftApprenticeshipsRequest request,
                   GetBulkUploadAddDraftApprenticeshipsResult response,
                   [Frozen] Mock<IMediator> mockMediator,
                   [Greedy] BulkUploadController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<BulkUploadAddDraftApprenticeshipsCommand>(x => x.ProviderId == request.ProviderId 
                                                                         && x.FileUploadLogId == request.FileUploadLogId
                                                                         && x.RplDataExtended == request.RplDataExtended 
                                                                         && x.UserInfo == request.UserInfo),
                    It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var controllerResult = await controller.AddDraftapprenticeships(request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_Result_Is_Returned_From_Mediator(
             BulkUploadAddDraftApprenticeshipsRequest request,
            [Greedy] BulkUploadController controller)
        {
            var controllerResult = await controller.AddDraftapprenticeships(request) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
