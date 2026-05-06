using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers;

public class ProviderEmailsControllerTests
{
    [Test, AutoData]
    public async Task SendEmailToProvider_SuccessfullyInvokesPASApi_ReturnsOk(int ukprn, ProviderEmailModel model)
    {
        Mock<IProviderAccountApiClient<ProviderAccountApiConfiguration>> apiMock = new();
        apiMock.Setup(a => a.PostWithResponseCode<object>(It.Is<PostProviderEmailRequest>(r => r.Ukprn == ukprn && r.Data == (object)model), false)).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.OK, string.Empty));
        ProviderEmailsController sut = new(apiMock.Object);

        var actual = await sut.SendEmailToProvider(ukprn, model, default);

        actual.Should().BeOfType<OkResult>();
    }

    [Test, AutoData]
    public async Task SendEmailToProvider_BadResponseFromPASApi_ThrowsException(int ukprn, ProviderEmailModel model)
    {
        Mock<IProviderAccountApiClient<ProviderAccountApiConfiguration>> apiMock = new();
        apiMock.Setup(a => a.PostWithResponseCode<object>(It.Is<PostProviderEmailRequest>(r => r.Ukprn == ukprn && r.Data == (object)model), false)).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.InternalServerError, string.Empty));
        ProviderEmailsController sut = new(apiMock.Object);

        Func<Task> action = () => sut.SendEmailToProvider(ukprn, model, default);

        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
