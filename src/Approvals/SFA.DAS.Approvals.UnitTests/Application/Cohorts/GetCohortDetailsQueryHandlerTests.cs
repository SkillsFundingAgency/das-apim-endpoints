using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Standard = SFA.DAS.Approvals.Types.Standard;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetCohortDetailsQueryHandlerTests
    {
        private GetCohortDetailsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private ServiceParameters _serviceParameters;

        private GetCohortResponse _cohort;
        private GetCohortDetailsQuery _query;

        private GetDraftApprenticeshipsResponse _draftApprenticeship;

        private GetEditDraftApprenticeshipDeliveryModelQueryResult _queryEditDraftResult;
        private List<Standard> _providerCourses;

        private List<string> _deliveryModels;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private Mock<IFjaaService> _fjaaService;
        private Mock<IProviderCoursesService> _providerCoursesService;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _cohort = fixture.Build<GetCohortResponse>()
                .With(x => x.WithParty, Party.Employer)
                .Create();

            _query = fixture.Create<GetCohortDetailsQuery>();

            _queryEditDraftResult = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQueryResult>();


            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipsResponse>();

            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));


            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.Is<GetDraftApprenticeshipsRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));


            _deliveryModelService = new Mock<IDeliveryModelService>();

            _fjaaService = new Mock<IFjaaService>();

            _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)_cohort.WithParty, _cohort.AccountId);

            _providerCourses = fixture.Create<List<Standard>>();
            _providerCoursesService = new Mock<IProviderCoursesService>();
            _providerCoursesService.Setup(x => x.GetCourses(It.Is<long>(id => id == _cohort.ProviderId)))
                .ReturnsAsync(_providerCourses);

            _handler = new GetCohortDetailsQueryHandler(_apiClient.Object, _serviceParameters, _fjaaService.Object, _providerCoursesService.Object);
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.LegalEntityName, result.LegalEntityName);
        }

        [Test]
        public async Task Handle_InvalidProviderCourseCodes_IsMapped_Empty()
        {
            _providerCourses.Clear();

            foreach (var courseCode in _draftApprenticeship.DraftApprenticeships.Select(x => x.CourseCode).Distinct())
            {
                _providerCourses.Add(new Standard(courseCode, $"test-{courseCode}"));
            }

            var result = await _handler.Handle(_query, CancellationToken.None);

            CollectionAssert.AreEqual(Enumerable.Empty<string>(), result.InvalidProviderCourseCodes);
        }

        [Test]
        public async Task Handle_InvalidProviderCourseCodes_IsMapped_NotEmpty()
        {
            _providerCourses.Clear();

            var result = await _handler.Handle(_query, CancellationToken.None);

            var expected = _draftApprenticeship.DraftApprenticeships.Select(x => x.CourseCode).Distinct();

            CollectionAssert.AreEqual(expected, result.InvalidProviderCourseCodes);
        }
    }
}
