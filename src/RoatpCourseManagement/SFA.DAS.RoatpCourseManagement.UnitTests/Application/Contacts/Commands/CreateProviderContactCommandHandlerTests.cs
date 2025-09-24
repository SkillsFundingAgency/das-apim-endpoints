using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Commands;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Contacts.Commands;

[TestFixture]
public class CreateProviderContactCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiCall_Created(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        CreateProviderContactCommandHandler sut,
        CreateProviderContactCommand command)
    {
        var apiResponse = new ApiResponse<int>(1, HttpStatusCode.Created, string.Empty);
        apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.Is<CreateProviderContactRequest>(r => r.Ukprn == command.Ukprn && r.Data == command
            && r.PostUrl == $"providers/{command.Ukprn}/contact?userId={HttpUtility.UrlEncode(command.UserId)}&userDisplayName={HttpUtility.UrlEncode(command.UserDisplayName)}"
        ), true)).ReturnsAsync(apiResponse);

        var response = await sut.Handle(command, new CancellationToken());
        apiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<CreateProviderContactRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true));
        response.Should().Be((int)HttpStatusCode.Created);
    }

    [Test, MoqAutoData]
    public async Task Handle_InvokesApiCall_Bad_Request(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        CreateProviderContactCommandHandler sut,
        CreateProviderContactCommand command)
    {
        var apiResponse = new ApiResponse<int>(0, HttpStatusCode.BadRequest, string.Empty);
        apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.Is<CreateProviderContactRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true)).ReturnsAsync(apiResponse);

        var response = await sut.Handle(command, new CancellationToken());
        apiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<CreateProviderContactRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true));
        response.Should().Be((int)HttpStatusCode.BadRequest);
    }
}

