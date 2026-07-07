using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class GetApprenticeshipsQueryHandlerTests
    {
        private GetApprenticeshipsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

        private GetApprenticeshipsResponse _apprenticeships;
        private GetApprenticeshipsQuery _query;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _apprenticeships = fixture.Build<GetApprenticeshipsResponse>()
                .Create();

            _query = fixture.Create<GetApprenticeshipsQuery>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(
                        r => r.ProviderId == _query.ProviderId
                        && r.SearchTerm == _query.SearchTerm
                        && r.EmployerName == _query.EmployerName
                        && r.CourseName == _query.CourseName
                        && r.Status == _query.Status
                        && r.StartDate == _query.StartDate
                        && r.EndDate == _query.EndDate
                        && r.Alert == _query.Alert
                        && r.ApprenticeConfirmationStatus == _query.ApprenticeConfirmationStatus
                        && r.DeliveryModel == _query.DeliveryModel
                        )))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipsResponse>(_apprenticeships, HttpStatusCode.OK, string.Empty));

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            var mapper = mappingConfig.CreateMapper();

            _handler = new GetApprenticeshipsQueryHandler(_apiClient.Object, mapper);
        }

        [Test]
        public async Task Handle_when_apprenticeships_are_returned()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_apprenticeships);
        }

        [Test]
        public async Task Handle_No_apprenticeships_returned()
        {
            _apiClient.Setup(x =>
                  x.GetWithResponseCode<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsRequest>()))
              .ReturnsAsync(new ApiResponse<GetApprenticeshipsResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var result = await _handler.Handle(_query, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}