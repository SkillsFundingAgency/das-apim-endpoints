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
    public class WhenValidatingBulkUpload
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Command_To_Validates_Csv_Records_From_Mediator(
                   BulkUploadValidateApimRequest request,
                   [Frozen] Mock<IMediator> mockMediator,
                   [Greedy] BulkUploadController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<ValidateBulkUploadRecordsCommand>(x => x.ProviderId == request.ProviderId 
                                                                && x.RplDataExtended == request.RplDataExtended
                                                                && x.FileUploadLogId == request.FileUploadLogId
                                                                && x.UserInfo == request.UserInfo),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.Validate(request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
