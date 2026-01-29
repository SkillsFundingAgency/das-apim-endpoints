using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class GetApprenticeshipsCSVQueryResultHandlerTests
    {
        private GetApprenticeshipsCSVQueryResultHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

        private GetApprenticeshipsResponse _apprenticeships;
        private GetApprenticeshipsCSVQuery _query;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _apprenticeships = fixture.Build<GetApprenticeshipsResponse>()
                .Create();

            _query = fixture.Create<GetApprenticeshipsCSVQuery>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsCSVRequest>(
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


            _handler = new GetApprenticeshipsCSVQueryResultHandler(_apiClient.Object, mapper);
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
                  x.GetWithResponseCode<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsCSVRequest>()))
              .ReturnsAsync(new ApiResponse<GetApprenticeshipsResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var result = await _handler.Handle(_query, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
