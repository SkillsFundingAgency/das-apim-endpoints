using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.BulkUpload
{
    public class WhenAddingBulkUploadDraftApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Apprenticeships_Are_Added(
         BulkUploadAddDraftApprenticeshipsCommand command,
         GetBulkUploadAddDraftApprenticeshipsResponse response,
         [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
         [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
         [Frozen] Mock<IAddCourseTypeDataToCsvService> addCourseTypeDataToCsvService,
         List<BulkUploadAddDraftApprenticeshipExtendedRequest> csvRecordsExtendedRequests,
         BulkUploadAddDraftApprenticeshipsCommandHandler handler
         )
        {
            var apiResponse = new ApiResponse<GetBulkUploadAddDraftApprenticeshipsResponse>(response, System.Net.HttpStatusCode.Created, string.Empty);
            
            apiClient.Setup(x => x.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>
                (It.Is<PostAddDraftApprenticeshipsRequest>(
                    x => x.ProviderId == command.ProviderId && (x.Data as BulkUploadAddDraftApprenticeshipsRequest).BulkUploadDraftApprenticeships == csvRecordsExtendedRequests
                                                            && (x.Data as BulkUploadAddDraftApprenticeshipsRequest).LogId == command.FileUploadLogId
                ), true)).ReturnsAsync(apiResponse);

            addCourseTypeDataToCsvService.Setup(x => x.MapAndAddCourseTypeData(command.BulkUploadAddDraftApprenticeships.ToList())).ReturnsAsync(csvRecordsExtendedRequests);

            var result = new BulkCreateReservationsWithNonLevyResult();
            foreach (var draftApprenticeship in command.BulkUploadAddDraftApprenticeships)
            {
                result.BulkCreateResults.Add(new BulkCreateReservationResult { ReservationId = System.Guid.NewGuid(), ULN = draftApprenticeship.Uln });
            }

            var reservationApiResponse = new ApiResponse<BulkCreateReservationsWithNonLevyResult>(result, System.Net.HttpStatusCode.OK, "");
            reservationApiClient.Setup(x => x.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(It.IsAny<PostBulkCreateReservationRequest>(), true)).ReturnsAsync(() => reservationApiResponse);

            var actual = await handler.Handle(command, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);

            actual.BulkUploadAddDraftApprenticeshipsResponse.Should().BeEquivalentTo(response.BulkUploadAddDraftApprenticeshipsResponse.Select(item => (BulkUploadAddDraftApprenticeshipsResult)item));
        }
    }
}
