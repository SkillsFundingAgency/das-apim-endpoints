using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Queries.GetProviderContact;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderContactControllerTests;
public class WhenGettingProviderContact
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        int ukprn,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderContactsController sut)
    {
        var email = "test@test.com";
        GetContactResponse response = new GetContactResponse { EmailAddress = email };
        var apiResponse = new ApiResponse<GetContactResponse>(response, HttpStatusCode.OK, "");

        mediator.Setup(x => x.Send(It.Is<GetContactQuery>(c => c.Ukprn == ukprn), new CancellationToken())).ReturnsAsync(apiResponse);

        var actual = await sut.GetProviderContact(ukprn) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual.Value as GetContactResponse;
        actualModel!.EmailAddress.Should().Be(email);
        actualModel.PhoneNumber.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_NotFound_Response_Returned(
        int ukprn,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderContactsController sut)
    {
        var apiResponse = new ApiResponse<GetContactResponse>(null, HttpStatusCode.NotFound, "");

        mediator.Setup(x => x.Send(It.Is<GetContactQuery>(c => c.Ukprn == ukprn), new CancellationToken())).ReturnsAsync(apiResponse);

        var response = await sut.GetProviderContact(ukprn);

        Assert.That(response, Is.Not.Null);


        var statusCodeResult = response as IStatusCodeActionResult;

        Assert.That(statusCodeResult!.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
    }

    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_BadRequest_Response_Returned(
        int ukprn,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderContactsController sut)
    {
        var apiResponse = new ApiResponse<GetContactResponse>(null, HttpStatusCode.BadRequest, "");

        mediator.Setup(x => x.Send(It.Is<GetContactQuery>(c => c.Ukprn == ukprn), new CancellationToken())).ReturnsAsync(apiResponse);

        var response = await sut.GetProviderContact(ukprn);

        Assert.That(response, Is.Not.Null);
        var statusCodeResult = response as IStatusCodeActionResult;

        Assert.That(statusCodeResult!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
    }
}
