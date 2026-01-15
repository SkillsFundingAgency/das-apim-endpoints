namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.DAS.LearnerData.Application.GetProviderRelationships;
    using SFA.DAS.LearnerData.Services;
    using SFA.DAS.SharedOuterApi.Configuration;
    using SFA.DAS.SharedOuterApi.InnerApi.Responses;
    using SFA.DAS.SharedOuterApi.Interfaces;

    [TestFixture]
    public class GetProviderRelationshipServiceTests
    {
        private GetProviderRelationshipService _testClass;
        private Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> _providerRelationshipApiClient;
        private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClient;
        private Mock<IFjaaApiClient<FjaaApiConfiguration>> _fjaaApiClient;

        [SetUp]
        public void SetUp()
        {
            _providerRelationshipApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
            _accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _fjaaApiClient = new Mock<IFjaaApiClient<FjaaApiConfiguration>>();
            _testClass = new GetProviderRelationshipService(_providerRelationshipApiClient.Object, _accountsApiClient.Object, _fjaaApiClient.Object);
        }

        [Test]
        public async Task CanCallGetEmployerDetails()
        {
            // Arrange
            var fixture = new Fixture();
            var employerDetails = fixture.Create<ConcurrentBag<EmployerDetails>>();
            var providerDetails = fixture.Create<GetProviderAccountLegalEntitiesResponse>();

            // Act
            await _testClass.GetEmployerDetails(employerDetails, providerDetails);

            employerDetails.Should().NotBeNull();
        }

        [Test]
        public async Task CannotCallGetEmployerDetailsWithNullEmployerDetails()
        {
            // Arrange
            var fixture = new Fixture();
            await FluentActions.Invoking(() => _testClass.GetEmployerDetails(default, fixture.Create<GetProviderAccountLegalEntitiesResponse>())).Should().ThrowAsync<ArgumentNullException>().WithParameterName("employerDetails");
        }

        [Test]
        public async Task CannotCallGetEmployerDetailsWithNullProviderDetails()
        {
            // Arrange
            var fixture = new Fixture();
            await FluentActions.Invoking(() => _testClass.GetEmployerDetails(fixture.Create<ConcurrentBag<EmployerDetails>>(), default)).Should().ThrowAsync<ArgumentNullException>().WithParameterName("providerDetails");
        }

        [Test]
        public async Task CanCallGetAllProviderRelationShipDetails()
        {
            // Arrange
            var fixture = new Fixture();
            var ukprn = fixture.Create<int>();

            // Act
            var result = await _testClass.GetAllProviderRelationShipDetails(ukprn);

            // Assert
            Assert.Fail("Create or modify test");
        }
    }
}