using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using ApprenticeshipEmployerType = SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments.ApprenticeshipEmployerType;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "CreateInitialIncentiveApplication")]
    public class CreateInitialIncentiveApplicationSteps
    {
        private readonly TestContext _context;
        private long _accountId;
        private long _accountLegalEntityId;
        private long[] _apprenticeshipIds;
        private ApprenticeshipResponse[] apprenticeshipResponses;
        private Guid _applicationId;
        private Fixture _fixture;
        private HttpResponseMessage _response;

        public CreateInitialIncentiveApplicationSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the employer has selected a few apprentices")]
        public void GivenTheEmployerHasSelectedAFewApprentices()
        {
            _accountId = _fixture.Create<long>();
            _accountLegalEntityId = _fixture.Create<long>();
            _applicationId = _fixture.Create<Guid>();
            _apprenticeshipIds = _fixture.CreateMany<long>(2).ToArray();

        }

        [Given(@"the apprenticeships are all found and valid")]
        public void GivenTheApprenticeshipsAreAllFoundAndValid()
        {

            apprenticeshipResponses = new ApprenticeshipResponse[2];

            apprenticeshipResponses[0] = _fixture.Build<ApprenticeshipResponse>().With(x => x.Id, _apprenticeshipIds[0])
                .With(x => x.EmployerAccountId, _accountId).With(x => x.AccountLegalEntityId, _accountLegalEntityId)
                .With(x => x.ApprenticeshipEmployerTypeOnApproval, ApprenticeshipEmployerType.Levy)
                .Create();
            apprenticeshipResponses[1] = _fixture.Build<ApprenticeshipResponse>().With(x => x.Id, _apprenticeshipIds[1])
                .With(x => x.EmployerAccountId, _accountId).With(x => x.AccountLegalEntityId, _accountLegalEntityId)
                .With(x => x.ApprenticeshipEmployerTypeOnApproval, ApprenticeshipEmployerType.Levy)
                .Create();

            SetResponseFromCommitmentsForApprenticeshipId(_apprenticeshipIds[0], apprenticeshipResponses[0]);
            SetResponseFromCommitmentsForApprenticeshipId(_apprenticeshipIds[1], apprenticeshipResponses[1]);
        }

        [When(@"the initial incentive application is saved")]
        public async Task WhenTheInitialIncentiveApplicationIsSaved()
        {
            SetupExpectedCreateIncentiveApplication();

            var request = new CreateApplicationRequest
            {
                ApplicationId = _applicationId,
                AccountId = _accountId,
                AccountLegalEntityId = _accountLegalEntityId,
                ApprenticeshipIds = _apprenticeshipIds
            };

            _response = await _context.OuterApiClient.PostAsync($"accounts/{_accountId}/applications", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
            _response.EnsureSuccessStatusCode();
        }


        [Then(@"the response should be Created")]
        public void ThenTheResponseShouldBeCreated()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        private void SetupExpectedCreateIncentiveApplication()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/applications")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Created));
        }

        private void SetResponseFromCommitmentsForApprenticeshipId(long id, ApprenticeshipResponse response)
        {
            _context.CommitmentsV2InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/apprenticeships/{id}")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response))
                );
        }
    }
}
