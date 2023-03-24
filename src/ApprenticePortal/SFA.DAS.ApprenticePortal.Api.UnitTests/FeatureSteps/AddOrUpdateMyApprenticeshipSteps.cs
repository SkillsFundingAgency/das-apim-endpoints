using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticePortal.Api.Controllers;
using SFA.DAS.ApprenticePortal.InnerApi.CommitmentsV2.Responses;
using SFA.DAS.ApprenticePortal.InnerApi.ProviderAccounts.Responses;
using SFA.DAS.ApprenticePortal.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests.FeatureSteps
{
    [Binding]
    [Scope(Feature = "AddOrUpdateMyApprenticeship")]
    public class AddOrUpdateMyApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Apprentice _apprentice;
        private ApprenticeshipDetailsResponse _apprenticeshipDetails;
        private TrainingProviderResponse _trainingProviderResponse;
        private MyApprenticeshipConfirmedRequest _confirmRequest;
        private MyApprenticeshipData _postedData;

        public AddOrUpdateMyApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Create<Apprentice>();
            _apprenticeshipDetails = _fixture.Create<ApprenticeshipDetailsResponse>();
            _trainingProviderResponse = _fixture.Create<TrainingProviderResponse>();
            _confirmRequest = _fixture.Build<MyApprenticeshipConfirmedRequest>()
                .With(x => x.CommitmentsApprenticeshipId, _apprenticeshipDetails.Id).Create();
        }

        [Given(@"there is an apprentice")]
        public void GivenThereIsAnApprentice()
        {
            _context.ApprenticeAccountsInnerApi.WithApprentice(_apprentice);
        }

        [Given(@"commitments apprenticeship exists")]
        public void GivenCommitmentsApprenticeshipExists()
        {
            _context.CommitmentsV2InnerApi.WithApprenticeshipsResponseForApprentice(_apprenticeshipDetails.Id, _apprenticeshipDetails);
        }

        [Given(@"training provider exists")]
        public void GivenTrainingProviderExists()
        {
            _context.TrainingProviderInnerApi.WithValidSearch(_apprenticeshipDetails.ProviderId, _trainingProviderResponse);
        }

        [Given(@"training provider has no trading name")]
        public void GivenTrainingProviderExistsButHasNoTradingName()
        {
            _trainingProviderResponse.TradingName = null;
        }

        [When(@"the MyApprenticeshipConfirmedRequest is posted")]
        public async Task WhenTheMyApprenticeshipConfirmedRequestIsPosted()
        {
            await _context.OuterApiClient.Post($"/apprentices/{_apprentice.ApprenticeId}/my-apprenticeship", _confirmRequest);
        }

        [Then(@"the call to save MyApprenticeship is called")]
        public void ThenTheCallToSaveMyApprenticeshipIsCalled()
        {
            var logEntry = _context.ApprenticeAccountsInnerApi.LogEntries.Should().Contain(x =>
                x.RequestMessage.Path == $"/apprentices/{_apprentice.ApprenticeId}/my-apprenticeship").Which;

            _postedData = JsonConvert.DeserializeObject<MyApprenticeshipData>(logEntry.RequestMessage.Body);
        }

        [Then(@"it contains all the information")]
        public void ThenItContainsAllTheInformation()
        {
            _postedData.ApprenticeshipId.Should().Be(_apprenticeshipDetails.Id);
            _postedData.Uln.Should().Be(_apprenticeshipDetails.Uln);
            _postedData.EmployerName.Should().Be(_apprenticeshipDetails.EmployerName);
            _postedData.StartDate.Should().Be(_apprenticeshipDetails.StartDate);
            _postedData.EndDate.Should().Be(_apprenticeshipDetails.EndDate);
            _postedData.TrainingProviderId.Should().Be(_apprenticeshipDetails.ProviderId);
            _postedData.TrainingCode.Should().Be(_apprenticeshipDetails.CourseCode);
            _postedData.StandardUId.Should().Be(_apprenticeshipDetails.StandardUId);
        }

        [Then(@"it contains the providers trading name")]
        public void ThenItContainsTheProvidersTradingName()
        {
            _postedData.TrainingProviderName.Should().Be(_trainingProviderResponse.TradingName);
        }

        [Then(@"it contains the providers legal name")]
        public void ThenItContainsTheProvidersLegalName()
        {
            _postedData.TrainingProviderName.Should().Be(_trainingProviderResponse.LegalName);
        }
    }
}
