using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ValidationOverride")]
    public class ValidationOverrideSteps
    {
        private readonly TestContext _context;
        private ValidationOverrideRequest _request;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private readonly Fixture _fixture;

        public ValidationOverrideSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to override validations for an apprenticeship application")]
        public void GivenTheCallerWantsToOverrideValidationsForAnApprenticeshipApplication()
        {
            _request = new ValidationOverrideRequest()
                {
                    ValidationOverrides = new List<ValidationOverride>()
                    {
                        new ValidationOverride
                        {
                            AccountLegalEntityId = _fixture.Create<long>(),
                            ULN = _fixture.Create<long>(),
                            ValidationSteps = new List<ValidationStep>(){
                                _fixture.Build<ValidationStep>()
                                .With(v => v.ValidationType, ValidationType.IsInLearning)
                                .With(v => v.ExpiryDate, DateTime.UtcNow.AddDays(10))                                
                                .Create()
                            }.ToArray(),
                            ServiceRequest = _fixture.Create<ServiceRequest>()
                        }
                    }.ToArray()
                };
        }

        [Given(@"the Employer Incentives Api receives the ValidationOverride request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheValidationOverrideRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.Accepted;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/validation-overrides")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)                        
                );
        }

        [When(@"the Outer Api receives the ValidationOverride request")]
        public async Task WhenTheOuterApiReceivesTheValidationOverrideRequest()
        {
           _response = await _context.OuterApiClient.PostAsJsonAsync("validation-overrides", _request);
        }

        [Then(@"the response of Accepted is returned")]
        public void ThenReturnAcceptedToTheCaller()
        {
            _response.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        }
    }
}
