namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    using AutoFixture;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.DAS.LearnerData.Application.GetProviderRelationships;
    using SFA.DAS.LearnerData.Services;
    using SFA.DAS.SharedOuterApi.Configuration;
    using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
    using SFA.DAS.SharedOuterApi.InnerApi.Requests.Rofjaa;
    using SFA.DAS.SharedOuterApi.InnerApi.Responses;
    using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
    using SFA.DAS.SharedOuterApi.InnerApi.Responses.Rofjaa;
    using SFA.DAS.SharedOuterApi.Interfaces;
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

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
            var accountsResponse = fixture.Create<GetAccountByIdResponse>();
            var agencyResponse = fixture.Create<GetAgencyResponse>();

            _accountsApiClient.Setup(t => t.Get<GetAccountByIdResponse>(It.IsAny<GetAccountByIdRequest>())).
                ReturnsAsync(accountsResponse);

            _fjaaApiClient.Setup(t => t.Get<GetAgencyResponse>(It.IsAny<GetAgencyQuery>())).
                ReturnsAsync(agencyResponse);

            // Act
            var details = await _testClass.GetEmployerDetails(providerDetails);

            details.Should().NotBeNull();
            details.Should().HaveCount(providerDetails.AccountProviderLegalEntities.Count);
        }

        [Test]
        public async Task CannotCallGetEmployerDetailsWithNullEmployerDetails()
        {
            // Arrange
            var fixture = new Fixture();
            await FluentActions.Invoking(() => _testClass.GetEmployerDetails(fixture.Create<GetProviderAccountLegalEntitiesResponse>())).Should().ThrowAsync<ArgumentNullException>().WithParameterName("employerDetails");
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