using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateOnlineDeliveryOption;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCourseEditControllerTests;
public class UpdateProviderCourseOnlineDeliveryOptionTests
{
    [Test, MoqAutoData]
    public async Task UpdateOnlineDeliveryOption_Success_ReturnsNoContent(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderCourseEditController sut,
            int ukprn, string larsCode, UpdateOnlineDeliveryOptionRequest request)
    {
        mediator.Setup(m => m.Send(It.Is<UpdateOnlineDeliveryOptionCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var result = await sut.UpdateOnlineDeliveryOption(ukprn, larsCode, request);

        (result as NoContentResult).Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task UpdateOnlineDeliveryOption_Failed_ReturnsRespectiveStatusCode(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderCourseEditController sut,
        int ukprn, string larsCode, UpdateOnlineDeliveryOptionRequest request)
    {
        mediator.Setup(m => m.Send(It.Is<UpdateOnlineDeliveryOptionCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestContentException("", HttpStatusCode.BadRequest));

        var result = await sut.UpdateOnlineDeliveryOption(ukprn, larsCode, request);
        var statusCodeResult = result as StatusCodeResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
