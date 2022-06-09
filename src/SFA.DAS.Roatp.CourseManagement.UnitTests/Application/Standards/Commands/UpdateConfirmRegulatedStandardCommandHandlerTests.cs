﻿using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateConfirmRegulatedStandard;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class UpdateConfirmRegulatedStandardCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateConfirmRegulatedStandardCommandHandler sut,
            UpdateConfirmRegulatedStandardCommand command,
            CancellationToken cancellationToken,
            UpdateConfirmRegulatedStandardRequest request)
        {
            apiClientMock.Setup(a => a.PostWithResponseCode<UpdateConfirmRegulatedStandardRequest>(It.IsAny<UpdateConfirmRegulatedStandardRequest>())).ReturnsAsync(new ApiResponse<UpdateConfirmRegulatedStandardRequest>(request, HttpStatusCode.NoContent, string.Empty));

            await sut.Handle(command, cancellationToken);

            apiClientMock.Verify(a => a.PostWithResponseCode<UpdateConfirmRegulatedStandardRequest>(It.IsAny<UpdateConfirmRegulatedStandardRequest>()), Times.Once);
        }
    }
}
