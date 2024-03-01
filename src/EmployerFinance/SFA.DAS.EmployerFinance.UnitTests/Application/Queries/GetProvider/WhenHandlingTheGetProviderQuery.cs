using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetProvider
{
    [TestFixture]
    public class WhenHandlingTheGetProviderQuery
    {
        private GetProviderQueryHandler _handler;
        private GetProviderQuery _query;
        private Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _providerCoursesApiClient;
        private GetProviderResponse _roatpServiceApiResponse;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _query = fixture.Create<GetProviderQuery>();
            _roatpServiceApiResponse = fixture.Create<GetProviderResponse>();
            _providerCoursesApiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
            _providerCoursesApiClient.Setup(x => x.Get<GetProviderResponse>(It.Is<SharedOuterApi.InnerApi.Requests.ProviderCourses.GetProviderRequest>(r => r.GetUrl.Equals($"api/providers/{_query.Id}"))))
                .ReturnsAsync(_roatpServiceApiResponse);

            _handler = new GetProviderQueryHandler( _providerCoursesApiClient.Object);
        }

        [Test]
        public async Task Then_Providers_Are_Returned_From_Course_Delivery_Api()
        {
            var actual = await _handler.Handle(_query, CancellationToken.None);

            actual.Should().BeEquivalentTo(_roatpServiceApiResponse);
        }

        [Test]
        public async Task When_Roatp_Toggle_Is_On_Then_Providers_Are_Returned_From_ProviderCoursesApi()
        {

            var actual = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(_roatpServiceApiResponse.Ukprn, Is.EqualTo(actual.Ukprn));
            Assert.That(_roatpServiceApiResponse.Name, Is.EqualTo(actual.Name));
            Assert.That(_roatpServiceApiResponse.ContactUrl, Is.EqualTo(actual.ContactUrl));
            Assert.That(_roatpServiceApiResponse.Phone, Is.EqualTo(actual.Phone));
            Assert.That(_roatpServiceApiResponse.Email, Is.EqualTo(actual.Email));
        }
    }
}