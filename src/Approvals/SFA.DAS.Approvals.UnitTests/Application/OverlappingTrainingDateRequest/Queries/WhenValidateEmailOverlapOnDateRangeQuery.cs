using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.OverlappingTrainingDateRequest.Queries;

public class WhenValidateEmailOverlapOnDateRangeQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
      ValidateEmailOverlapQuery query,
      ValidateEmailOverlapQueryResponse apiResponse,
      [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
      ValidateEmailOverlapQueryHandler handler
      )
    {
        var response = new ApiResponse<ValidateEmailOverlapQueryResponse>(apiResponse, System.Net.HttpStatusCode.OK, string.Empty);

        apiClient
            .Setup(x => x.GetWithResponseCode<ValidateEmailOverlapQueryResponse>(It.IsAny<ValidateEmailOverlapRequest>()))
            .ReturnsAsync(response);

        var actual = await handler.Handle(query, CancellationToken.None);
        Assert.That(actual, Is.Not.Null);

        apiClient
         .Verify(x => x.GetWithResponseCode<ValidateEmailOverlapQueryResponse>(It.Is<ValidateEmailOverlapRequest>(x =>
         x.DraftApprenticeshipId == query.DraftApprenticeshipId && x.CohortId == query.CohortId && x.Email == query.Email 
         && x.StartDate == query.StartDate && x.EndDate == query.EndDate )), Times.Once);
    }
}