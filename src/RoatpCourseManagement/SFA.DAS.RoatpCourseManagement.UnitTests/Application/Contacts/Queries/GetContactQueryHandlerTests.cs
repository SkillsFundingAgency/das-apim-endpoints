using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Queries.GetProviderContact;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Contacts.Queries;

[TestFixture]
public class GetContactQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_CallsInnerApi_ReturnsValidResponse(
        GetContactResponse apiResponse,
        GetContactQuery query,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetContactQueryHandler sut)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetContactResponse>(It.Is<GetContactRequest>(c =>
                c.GetUrl.Equals(new GetContactRequest(query.Ukprn).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetContactResponse>(apiResponse, HttpStatusCode.OK, ""));

        var result = await sut.Handle(query, new CancellationToken());

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = result.Body;
        response.EmailAddress.Should().Be(apiResponse.EmailAddress);
        response.PhoneNumber.Should().Be(apiResponse.PhoneNumber);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_CallsInnerApi_ReturnsNotFoundResponse(
        GetContactQuery query,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetContactQueryHandler sut)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetContactResponse>(It.Is<GetContactRequest>(c =>
                c.GetUrl.Equals(new GetContactRequest(query.Ukprn).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetContactResponse>(null, HttpStatusCode.NotFound, ""));

        var result = await sut.Handle(query, new CancellationToken());

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_CallsInnerApi_ReturnsBadRequestResponse(
        GetContactQuery query,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetContactQueryHandler sut)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetContactResponse>(It.Is<GetContactRequest>(c =>
                c.GetUrl.Equals(new GetContactRequest(query.Ukprn).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetContactResponse>(null, HttpStatusCode.BadRequest, ""));

        var result = await sut.Handle(query, new CancellationToken());

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}