using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.GetMetricsDataByLepsCode;

public class WhenGettingMetricsDataByLepsCode
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_The_MetricData_Is_Returned(
        GetMetricsDataByLepsCodeQuery query,
        GetMetricsDataByLepsCodeResponse apiResponse,
        [Frozen] Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>> apiClient,
        GetMetricsDataByLepsCodeQueryHandler handler
    )
    {
        apiClient.Setup(x => x.GetWithResponseCode<GetMetricsDataByLepsCodeResponse>(It.Is<GetMetricsDataByLepsCodeRequest>(x => x.LepsCode == query.LepsCode))).ReturnsAsync(new ApiResponse<GetMetricsDataByLepsCodeResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().BeEquivalentTo(apiResponse);
    }
}