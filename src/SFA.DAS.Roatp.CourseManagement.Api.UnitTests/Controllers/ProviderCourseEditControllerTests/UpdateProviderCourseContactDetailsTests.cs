﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Api.Controllers;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.UnitTests.Controllers.ProviderCourseEditControllerTests
{
    [TestFixture]
    public class UpdateProviderCourseContactDetailsTests
    {
        [Test, MoqAutoData]
        public async Task UpdateProviderCourseContactDetails_Success_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseEditController sut,
            int ukprn, int larsCode, UpdateContactDetailsCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateContactDetailsCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.NoContent);

            var result = await sut.UpdateProviderCourseContactDetails(ukprn, larsCode, command);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task UpdateProviderCourseContactDetails_Failed_ReturnsRespectiveStatusCode(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseEditController sut,
            int ukprn, int larsCode, UpdateContactDetailsCommand command)
        {
            mediator.Setup(m => m.Send(It.Is<UpdateContactDetailsCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.BadRequest);

            var result = await sut.UpdateProviderCourseContactDetails(ukprn, larsCode, command);
            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }
    }
}
