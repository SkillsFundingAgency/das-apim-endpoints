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
    public class WhenAddingBulkUploadDraftApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Apprenticeships_Are_Added(
         BulkUploadAddDraftApprenticeshipsCommand query,
         GetBulkUploadAddDraftApprenticeshipsResponse response,
         [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
         BulkUploadAddDraftApprenticeshipsCommandHandler handler
         )
        {
            var apiResponse = new ApiResponse<GetBulkUploadAddDraftApprenticeshipsResponse>(response, System.Net.HttpStatusCode.Created, string.Empty);
            
            apiClient.Setup(x => x.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>
                (It.Is<PostAddDraftApprenticeshipsRequest>(
                    x => x.ProviderId == query.ProviderId && (x.Data as BulkUploadAddDraftApprenticeshipsRequest).BulkUploadDraftApprenticeships == query.BulkUploadAddDraftApprenticeships
                ))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);
            Assert.IsNotNull(actual);

            actual.BulkUploadAddDraftApprenticeshipsResponse.Should().BeEquivalentTo(response.BulkUploadAddDraftApprenticeshipsResponse.Select(item => (BulkUploadAddDraftApprenticeshipsResult)item));
        }
    }
}
