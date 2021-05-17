using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.ApprenticeshipDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ApprenticeshipDetails")]
    public class ApprenticeshipDetailsSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture;
        private long _accountId;
        private long _accountLegalEntityId;
        private long[] _apprenticeshipIds;
        private ApprenticeshipResponse[] _apprenticeshipResponses;
        private Guid _applicationId;
        private HttpResponseMessage _response;
        private ApprenticeshipDetailsRequest _confirmEmploymentDetailsRequest;

        public ApprenticeshipDetailsSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"an employer is applying for the New Apprenticeship Incentive")]
        public void GivenAnEmployerIsApplyingForTheNewApprenticeshipIncentive()
        {
            _accountId = _fixture.Create<long>();
            _accountLegalEntityId = _fixture.Create<long>();
            _applicationId = _fixture.Create<Guid>();
            _apprenticeshipIds = _fixture.CreateMany<long>(2).ToArray();
        }

        [Given(@"employer has selected the apprenticeships for the application")]
        public async Task GivenEmployerHasSelectedTheApprenticeshipsForTheApplication()
        {
            _apprenticeshipResponses = new ApprenticeshipResponse[2];

            _apprenticeshipResponses[0] = _fixture.Build<ApprenticeshipResponse>().With(x => x.Id, _apprenticeshipIds[0])
                .With(x => x.EmployerAccountId, _accountId).With(x => x.AccountLegalEntityId, _accountLegalEntityId)
                .With(x => x.ApprenticeshipEmployerTypeOnApproval, ApprenticeshipEmployerType.Levy)
                .Create();
            _apprenticeshipResponses[1] = _fixture.Build<ApprenticeshipResponse>().With(x => x.Id, _apprenticeshipIds[1])
                .With(x => x.EmployerAccountId, _accountId).With(x => x.AccountLegalEntityId, _accountLegalEntityId)
                .With(x => x.ApprenticeshipEmployerTypeOnApproval, ApprenticeshipEmployerType.Levy)
                .Create();

            SetResponseFromCommitmentsForApprenticeshipId(_apprenticeshipIds[0], _apprenticeshipResponses[0]);
            SetResponseFromCommitmentsForApprenticeshipId(_apprenticeshipIds[1], _apprenticeshipResponses[1]);

            SetupExpectedCreateAndUpdateIncentiveApplication();

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

        [When(@"the employer confirms the employment start dates for their apprentices")]
        public async Task WhenTheEmployerConfirmsTheEmploymentStartDatesForTheirApprentices()
        {
            _confirmEmploymentDetailsRequest = new ApprenticeshipDetailsRequest
            {
                AccountId = _accountId,
                ApplicationId = _applicationId,
                ApprenticeshipDetails = new List<ApprenticeDetailsDto>()
            };
            foreach(var apprenticeId in _apprenticeshipIds)
            {
                _confirmEmploymentDetailsRequest.ApprenticeshipDetails.Add(new ApprenticeDetailsDto { ApprenticeId = apprenticeId, EmploymentStartDate = _fixture.Create<DateTime>()});
            }

            var url = $"accounts/{_accountId}/applications/{_applicationId}/apprenticeships";

            _response = await _context.OuterApiClient.PatchAsync(url, new StringContent(JsonSerializer.Serialize(_confirmEmploymentDetailsRequest), Encoding.UTF8, "application/json"));
        }

        [Then(@"the application is updated with the apprentices employment details")]
        public void ThenTheApplicationIsUpdatedWithTheApprenticesEmploymentDetails()
        {
            _response.EnsureSuccessStatusCode();
        }

        private void SetupExpectedCreateAndUpdateIncentiveApplication()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/applications")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Created));

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/applications/{_applicationId}")
                        .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));
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
