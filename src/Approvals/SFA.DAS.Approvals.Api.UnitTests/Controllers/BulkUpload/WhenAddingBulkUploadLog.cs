using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
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

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}