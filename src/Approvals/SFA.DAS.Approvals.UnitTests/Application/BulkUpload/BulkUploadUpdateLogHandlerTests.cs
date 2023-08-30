using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.BulkUpload
{
    [TestFixture]
    public class BulkUploadUpdateLogHandlerTests
    {
        private BulkUploadUpdateLogCommandHandler _handler;
        private BulkUploadUpdateLogCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<BulkUploadUpdateLogCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _handler = new BulkUploadUpdateLogCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Bulk_Log_Updated()
        {
            var expectedResponse = _fixture.Create<GetBulkUploadUpdateLogResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<GetBulkUploadUpdateLogResponse>(
                    It.Is<PostBulkUploadUpdateLogRequest>(r =>
                            ((BulkUploadUpdateLogRequest)r.Data).ProviderId == _request.ProviderId &&
                            ((BulkUploadUpdateLogRequest)r.Data).LogId == _request.LogId
                        ), true
                )).ReturnsAsync(new ApiResponse<GetBulkUploadUpdateLogResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}