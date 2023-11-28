using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.BulkUpload
{
    public class WhenUpdatingBulkUploadLogWithErrorContent
    {
        [Test, MoqAutoData]
        public async Task Add_Log_Test(
            BulkUploadLogUpdateWithErrorContentRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] BulkUploadController controller)
        {
            await controller.UpdateLogWithErrorContent(1234, request);
            mockMediator.Verify(x => x.Send(It.Is<BulkUploadLogUpdateWithErrorContentCommand>(p => p.LogId == 1234 && p.ProviderId == request.ProviderId && p.ErrorContent == request.ErrorContent), CancellationToken.None));
        }
    }
}