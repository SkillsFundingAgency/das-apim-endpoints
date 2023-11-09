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
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.BulkUpload
{
    public class WhenAddLogBulkUpload
    {
        [Test, MoqAutoData]
        public async Task Add_Log_Test(
                   BulkUploadAddLogRequest request,
                   BulkUploadAddLogResult response,
                   [Frozen] Mock<IMediator> mockMediator,
                   [Greedy] BulkUploadController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<BulkUploadAddLogCommand>(x => x.ProviderId == request.ProviderId && x.FileContent == request.FileContent && x.RowCount == request.RowCount && x.RplCount == request.RplCount && x.FileName == request.FileName),
                    It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var controllerResult = await controller.AddLog(request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}