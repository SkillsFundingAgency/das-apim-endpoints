using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AparRegister.Application.Queries.ProviderStatusEvents;
using SFA.DAS.AparRegister.InnerApi.Requests;
using SFA.DAS.AparRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AparRegister.UnitTests.Application.Queries.ProviderStatusEvents;

public class GetProviderStatusEventsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_InvokesApiClient(
        IEnumerable<ProviderStatusEvent> apiResponse,
        GetProviderStatusEventsQuery expected,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetProviderStatusEventsQueryHandler sut,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<ProviderStatusEvent>>(It.IsAny<GetProviderStatusEventsRequest>())).ReturnsAsync(new ApiResponse<IEnumerable<ProviderStatusEvent>>(apiResponse, System.Net.HttpStatusCode.OK, string.Empty));

        await sut.Handle(expected, cancellationToken);

        apiClientMock.Verify(a => a.GetWithResponseCode<IEnumerable<ProviderStatusEvent>>(It.Is<GetProviderStatusEventsRequest>(actual => actual.SinceEventId == expected.SinceEventId && actual.PageNumber == expected.PageNumber && actual.PageSize == expected.PageSize)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handler_ReturnsExpectedResult(
        IEnumerable<ProviderStatusEvent> expected,
        GetProviderStatusEventsQuery query,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetProviderStatusEventsQueryHandler sut,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<ProviderStatusEvent>>(It.IsAny<GetProviderStatusEventsRequest>())).ReturnsAsync(new ApiResponse<IEnumerable<ProviderStatusEvent>>(expected, System.Net.HttpStatusCode.OK, string.Empty));

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test, MoqAutoData]
    public async Task Handler_InvalidResponseCode_ThrowsException(
        GetProviderStatusEventsQuery query,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetProviderStatusEventsQueryHandler sut,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<ProviderStatusEvent>>(It.IsAny<GetProviderStatusEventsRequest>())).ReturnsAsync(new ApiResponse<IEnumerable<ProviderStatusEvent>>(null, System.Net.HttpStatusCode.InternalServerError, string.Empty));

        var action = () => sut.Handle(query, cancellationToken);

        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
