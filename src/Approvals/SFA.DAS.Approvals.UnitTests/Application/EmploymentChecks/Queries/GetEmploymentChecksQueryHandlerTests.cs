using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.EmploymentChecks.Queries.GetEmploymentChecksQuery;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Requests;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.EmploymentChecks.Queries;

public class GetEmploymentChecksQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_Calls_Client_With_Request_And_Returns_Checks(
        GetEmploymentChecksQuery query,
        List<EvsCheckResponse> evsChecks,
        [Frozen] Mock<IEmploymentCheckApiClient<EmploymentCheckConfiguration>> client,
        GetEmploymentChecksQueryHandler handler)
    {
        // Arrange
        client
            .Setup(x => x.GetWithResponseCode<List<EvsCheckResponse>>(It.IsAny<GetEmploymentCheckLearnersRequest>()))
            .ReturnsAsync(new ApiResponse<List<EvsCheckResponse>>(evsChecks, HttpStatusCode.OK, null));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Checks.Should().BeEquivalentTo(evsChecks);
        client.VerifyAll();
        client.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task And_Client_Returns_Empty_Body_Then_Returns_Empty_List(
        GetEmploymentChecksQuery query,
        [Frozen] Mock<IEmploymentCheckApiClient<EmploymentCheckConfiguration>> client,
        GetEmploymentChecksQueryHandler handler)
    {
        // Arrange
        client
            .Setup(x => x.GetWithResponseCode<List<EvsCheckResponse>>(It.IsAny<GetEmploymentCheckLearnersRequest>()))
            .ReturnsAsync(new ApiResponse<List<EvsCheckResponse>>(null, HttpStatusCode.OK, null));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Checks.Should().NotBeNull().And.BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task And_Client_Returns_Error_Status_Then_Throws(
        GetEmploymentChecksQuery query,
        [Frozen] Mock<IEmploymentCheckApiClient<EmploymentCheckConfiguration>> client,
        GetEmploymentChecksQueryHandler handler)
    {
        // Arrange
        client
            .Setup(x => x.GetWithResponseCode<List<EvsCheckResponse>>(It.IsAny<GetEmploymentCheckLearnersRequest>()))
            .ReturnsAsync(new ApiResponse<List<EvsCheckResponse>>(null, HttpStatusCode.InternalServerError, "Inner API error"));

        // Act
        var act = () => handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Employment check API error*");
    }
}
