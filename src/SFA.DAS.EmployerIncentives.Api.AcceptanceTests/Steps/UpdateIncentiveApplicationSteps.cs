using AutoFixture;
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

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "UpdateIncentiveApplication")]
    public class UpdateIncentiveApplicationSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture;
        private long _accountId;
        private long _accountLegalEntityId;
        private long[] _apprenticeshipIds;
        private ApprenticeshipResponse[] _apprenticeshipResponses;
        private Guid _applicationId;
        private HttpResponseMessage _response;

        public UpdateIncentiveApplicationSteps(TestContext context)
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

        [When(@"employer has changed selected apprenticeships for the application")]
        public void WhenEmployerHasChangedSelectedApprenticeshipsForTheApplication()
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
        }

        [Then(@"the application is updated with new selection of apprenticeships")]
        public async Task ThenTheApplicationIsUpdatedWithNewSelectionOfApprenticeships()
        {
            SetupExpectedUpdateIncentiveApplication();

            var request = new UpdateApplicationRequest
            {
                ApplicationId = _applicationId,
                AccountId = _accountId,
                ApprenticeshipIds = _apprenticeshipIds
            };

            _response = await _context.OuterApiClient.PutAsync($"accounts/{_accountId}/applications", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
            _response.EnsureSuccessStatusCode();
        }

        private void SetupExpectedUpdateIncentiveApplication()
        {
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
