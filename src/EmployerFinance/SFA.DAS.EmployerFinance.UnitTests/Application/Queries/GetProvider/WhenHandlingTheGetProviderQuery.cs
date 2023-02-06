using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetProvider
{
    [TestFixture]
    public class WhenHandlingTheGetProviderQuery
    {
        private GetProviderQueryHandler _handler;
        private GetProviderQuery _query;
        private Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> _courseDeliveryApiClient;
        private GetProvidersListItem _courseDeliveryApiResponse;
        private Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _providerCoursesApiClient;
        private GetProviderResponse _roatpServiceApiResponse;
        private FeatureToggles _featureToggles;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _query = fixture.Create<GetProviderQuery>();
            _courseDeliveryApiResponse = fixture.Create<GetProvidersListItem>();
            _roatpServiceApiResponse = fixture.Create<GetProviderResponse>();

            _courseDeliveryApiClient = new Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>>();
            _courseDeliveryApiClient.Setup(x =>
                    x.Get<GetProvidersListItem>(
                        It.Is<GetProviderRequest>(c => c.GetUrl.Equals($"api/providers/{_query.Id}"))))
                .ReturnsAsync(_courseDeliveryApiResponse);

            _providerCoursesApiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
            _providerCoursesApiClient.Setup(x => x.Get<GetProviderResponse>(It.Is<SharedOuterApi.InnerApi.Requests.ProviderCourses.GetProviderRequest>(r => r.GetUrl.Equals($"api/providers/{_query.Id}"))))
                .ReturnsAsync(_roatpServiceApiResponse);

            _featureToggles = new FeatureToggles();

            _handler = new GetProviderQueryHandler(_courseDeliveryApiClient.Object, _providerCoursesApiClient.Object, _featureToggles);
        }

        [Test]
        public async Task Then_Providers_Are_Returned_From_Course_Delivery_Api()
        {
            var actual = await _handler.Handle(_query, CancellationToken.None);

            actual.Should().BeEquivalentTo(_courseDeliveryApiResponse);
        }

        [Test]
        public async Task When_Roatp_Toggle_Is_On_Then_Providers_Are_Returned_From_ProviderCoursesApi()
        {
            _featureToggles.RoatpProvidersEnabled = true;

            var actual = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_roatpServiceApiResponse.Ukprn, actual.Ukprn);
            Assert.AreEqual(_roatpServiceApiResponse.LegalName, actual.Name);
            Assert.AreEqual(_roatpServiceApiResponse.Website, actual.ContactUrl);
            Assert.AreEqual(_roatpServiceApiResponse.Phone, actual.Phone);
            Assert.AreEqual(_roatpServiceApiResponse.Email, actual.Email);
        }
    }
}