using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
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
    public class WhenAddAndApprovingBulkUploadDraftApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Apprenticeships_Are_Added_And_Approved(
       BulkUploadAddAndApproveDraftApprenticeshipsCommand command,
       BulkUploadAddAndApproveDraftApprenticeshipsResponse response,
       [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
       [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
       BulkUploadAddAndApproveDraftApprenticeshipsCommandHandler handler
       )
        {
            var apiResponse = new ApiResponse<BulkUploadAddAndApproveDraftApprenticeshipsResponse>(response, System.Net.HttpStatusCode.Created, string.Empty);

            apiClient.Setup(x => x.PostWithResponseCode<BulkUploadAddAndApproveDraftApprenticeshipsResponse>
                (It.Is<PostAddAndApproveDraftApprenticeshipsRequest>(
                    x => x.ProviderId == command.ProviderId &&
                    (x.Data as BulkUploadAddAndApproveDraftApprenticeshipsRequest).BulkUploadAddAndApproveDraftApprenticeships == command.BulkUploadAddAndApproveDraftApprenticeships &&
                    (x.Data as BulkUploadAddAndApproveDraftApprenticeshipsRequest).LogId == command.FileUploadLogId)
                , true)).ReturnsAsync(apiResponse);

            var result = new BulkCreateReservationsWithNonLevyResult();
            foreach (var draftApprenticeship in command.BulkUploadAddAndApproveDraftApprenticeships)
            {
                result.BulkCreateResults.Add(new BulkCreateReservationResult { ReservationId = System.Guid.NewGuid(), ULN = draftApprenticeship.Uln });
            }

            var reservationApiResponse = new ApiResponse<BulkCreateReservationsWithNonLevyResult>(result, System.Net.HttpStatusCode.OK, "");
            reservationApiClient.Setup(x => x.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(It.IsAny<PostBulkCreateReservationRequest>(), true)).ReturnsAsync(() => reservationApiResponse);

            var actual = await handler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(actual);

            actual.BulkUploadAddAndApproveDraftApprenticeshipResponse.Should().BeEquivalentTo(response.BulkUploadAddAndApproveDraftApprenticeshipResponse.Select(item => (BulkUploadAddDraftApprenticeshipsResult)item));
        }
    }
}