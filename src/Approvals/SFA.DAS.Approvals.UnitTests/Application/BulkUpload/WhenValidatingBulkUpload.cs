using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.BulkUpload
{
    public class WhenValidatingBulkUpload
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
          ValidateBulkUploadRecordsCommand query,
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
          [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
          ValidateBulkUploadRecordsCommandHandler handler
          )
        {
            var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.OK, string.Empty);

            var reservationValidationResult = new BulkReservationValidationResults();
            var reservationApiResponse = new ApiResponse<BulkReservationValidationResults>(reservationValidationResult, System.Net.HttpStatusCode.OK, "");
            reservationApiClient.Setup(x => x.PostWithResponseCode<BulkReservationValidationResults>(It.IsAny<PostValidateReservationRequest>(), true)).ReturnsAsync(() => reservationApiResponse);

            apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostValidateBulkUploadRequest>(), true)).ReturnsAsync(response);
            var actual = await handler.Handle(query, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);
        }
    }
}
