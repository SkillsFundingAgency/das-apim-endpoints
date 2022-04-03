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
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.BulkUpload
{
    public class WhenAddAndApproveBulkUploadDraftApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_Add_And_Approve_Apprentices_From_Mediator(
                   BulkUploadAddAndApproveDraftApprenticeshipsRequest request,
                   BulkUploadAddAndApproveDraftApprenticeshipsResult response,
                   [Frozen] Mock<IMediator> mockMediator,
                   [Greedy] BulkUploadController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<BulkUploadAddAndApproveDraftApprenticeshipsCommand>(x => x.ProviderId == request.ProviderId && x.UserInfo == request.UserInfo),
                    It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var controllerResult = await controller.AddAndApprove(request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_Result_Is_Returned_From_Mediator(
             BulkUploadAddAndApproveDraftApprenticeshipsRequest request,
            [Greedy] BulkUploadController controller)
        {
            var controllerResult = await controller.AddAndApprove(request) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
