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

namespace SFA.DAS.Approvals.UnitTests.Application.OverlappingTrainingDateRequest.Queries
{
    public class WhenValidateUlnOverlapOnStartDateQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
          ValidateUlnOverlapOnStartDateQuery query,
          ValidateUlnOverlapOnStartDateResponse apiResponse,
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
          ValidateUlnOverlapOnStartDateQueryHandler handler
          )
        {
            var response = new ApiResponse<ValidateUlnOverlapOnStartDateResponse>(apiResponse, System.Net.HttpStatusCode.OK, string.Empty);

            apiClient
                .Setup(x => x.GetWithResponseCode<ValidateUlnOverlapOnStartDateResponse>(It.IsAny<ValidateUlnOverlapOnStartDateQueryRequest>()))
                .ReturnsAsync(response);

            var actual = await handler.Handle(query, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);

            apiClient
             .Verify(x => x.GetWithResponseCode<ValidateUlnOverlapOnStartDateResponse>(It.Is<ValidateUlnOverlapOnStartDateQueryRequest>(x =>
             x.Uln == query.Uln && x.ProviderId == query.ProviderId)), Times.Once);
        }
    }
}