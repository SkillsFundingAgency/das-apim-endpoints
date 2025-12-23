using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.BulkUpload;

public class WhenValidatingBulkUpload
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
        ValidateBulkUploadRecordsCommand query,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<IProviderStandardsService> providerStandardsService,
        [Frozen] Mock<IBulkCourseMetadataService> bulkCourseMetadataService,
        [Frozen] Mock<IAddCourseTypeDataToCsvService> addCourseTypeDataToCsvService,
        List<BulkUploadAddDraftApprenticeshipExtendedRequest> csvRecordsExtendedRequests,
        ValidateBulkUploadRecordsCommandHandler handler
    )
    {
        var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.OK, string.Empty);

        var reservationValidationResult = new BulkReservationValidationResults();
        var reservationApiResponse = new ApiResponse<BulkReservationValidationResults>(reservationValidationResult, System.Net.HttpStatusCode.OK, "");
        reservationApiClient.Setup(x => x.PostWithResponseCode<BulkReservationValidationResults>(It.IsAny<PostValidateReservationRequest>(), true)).ReturnsAsync(() => reservationApiResponse);
        addCourseTypeDataToCsvService.Setup(x => x.MapAndAddCourseTypeData(query.CsvRecords)).ReturnsAsync(csvRecordsExtendedRequests);

        var providerStandardsData = new ProviderStandardsData();
        providerStandardsService.Setup(x => x.GetStandardsData(query.ProviderId)).ReturnsAsync(providerStandardsData);

        var otjTrainingHours = new Dictionary<string, int?>();
        bulkCourseMetadataService.Setup(x => x.GetOtjTrainingHoursForBulkUploadAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(otjTrainingHours);

        apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostValidateBulkUploadRequest>(), true)).ReturnsAsync(response);
            
        var actual = await handler.Handle(query, CancellationToken.None);
            
        actual.Should().NotBeNull();
            
        bulkCourseMetadataService.Verify(x => x.GetOtjTrainingHoursForBulkUploadAsync(It.IsAny<IEnumerable<string>>()), Times.Once);
            
        apiClient.Verify(x => x.PostWithResponseCode<object>(It.Is<PostValidateBulkUploadRequest>(r => 
            ((BulkUploadValidateApiRequest)r.Data).OtjTrainingHours == otjTrainingHours &&
            ((BulkUploadValidateApiRequest)r.Data).CsvRecords == csvRecordsExtendedRequests), true), Times.Once);
    }
}