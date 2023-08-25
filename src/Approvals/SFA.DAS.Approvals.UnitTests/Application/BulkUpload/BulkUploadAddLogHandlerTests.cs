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
    public class BulkUploadAddLogHandlerTests
    {
        private BulkUploadAddLogCommandHandler _handler;
        private BulkUploadAddLogCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<BulkUploadAddLogCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _handler = new BulkUploadAddLogCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Bulk_Log_Created()
        {
            var expectedResponse = _fixture.Create<GetBulkUploadAddLogResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<GetBulkUploadAddLogResponse>(
                    It.Is<PostBulkUploadAddLogRequest>(r =>
                            ((BulkUploadAddLogRequest)r.Data).RowCount == _request.RowCount &&
                            ((BulkUploadAddLogRequest)r.Data).RplCount == _request.RplCount &&
                            ((BulkUploadAddLogRequest)r.Data).FileName == _request.FileName &&
                            ((BulkUploadAddLogRequest)r.Data).FileContent == _request.FileContent 
                        ), true
                )).ReturnsAsync(new ApiResponse<GetBulkUploadAddLogResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}