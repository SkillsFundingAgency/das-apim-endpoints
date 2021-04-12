using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Registration;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetRegistration")]
    public class GetRegistrationSteps
    {
        private readonly TestContext _context;
        private Guid _apprenticeId;
        private RegistrationResponse _registrationResponse;
        private Fixture _f;

        public GetRegistrationSteps(TestContext context)
        {
            _context = context;
            _f = new Fixture();

            _apprenticeId = _f.Create<Guid>();
            _registrationResponse =
                _f.Build<RegistrationResponse>()
                    .With(m => m.ApprenticeId, _apprenticeId)
                    .Create();
        }

        [Given(@"no registration exists")]
        public void GivenNoRegistrationExists()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations/{_apprenticeId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NotFound)
                );
        }

        [Given(@"a registration exists")]
        public void GivenARegistrationExists()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations/{_apprenticeId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(_registrationResponse))
                );
        }

        [When(@"retrieving registration details")]
        public Task WhenRetrievingRegistrationDetails()
        {
            return _context.OuterApiClient.Get($"registrations/{_apprenticeId}");
        }

        [Then(@"not found is returned")]
        public void ThenNotFoundIsReturned()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then(@"Ok is returned")]
        public void ThenOkIsReturned()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the response contains the expected details")]
        public async Task ThenTheResponseContainsTheExpectedDetails()
        {
            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RegistrationResponse>(content);
            response.Should().NotBeNull();
            response.Email.Should().Be(_registrationResponse.Email);
            response.ApprenticeId.Should().Be(_apprenticeId);
        }
    }
}