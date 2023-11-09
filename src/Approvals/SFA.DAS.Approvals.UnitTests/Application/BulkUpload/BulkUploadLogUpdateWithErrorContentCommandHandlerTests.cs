using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.BulkUpload
{
    [TestFixture]
    public class BulkUploadLogUpdateWithErrorContentCommandHandlerTests
    {
        private BulkUploadLogUpdateWithErrorContentCommandHandler _handler;
        private BulkUploadLogUpdateWithErrorContentCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<BulkUploadLogUpdateWithErrorContentCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _handler = new BulkUploadLogUpdateWithErrorContentCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_FileUploadLog_Created()
        {
            await _handler.Handle(_request, CancellationToken.None);
            _commitmentsApiClient.Verify(x => x.Put(It.Is<PutBulkUploadLogUpdateWithErrorContentRequest>(p =>
                p.LogId == _request.LogId && p.ProviderId == _request.ProviderId &&
                ((BulkUploadLogUpdateWithErrorContentRequest) p.Data).ErrorContent == _request.ErrorContent)));
        }
    }
}