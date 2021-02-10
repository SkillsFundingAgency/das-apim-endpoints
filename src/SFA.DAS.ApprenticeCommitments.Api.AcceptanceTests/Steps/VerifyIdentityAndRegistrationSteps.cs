using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Api.ErrorHandler;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "VerifyIdentityAndRegistration")]
    public class VerifyIdentityAndRegistrationSteps
    {
        private readonly TestContext _context;
        private VerifyIdentityRegistrationCommand _command;
        private Fixture _f;
        private string _validEmail;
        private string _validNI;
        private Guid _registrationId;
        private Guid _userIdentityId;
        private ErrorItem _errorItem;

        public VerifyIdentityAndRegistrationSteps(TestContext context)
        {
            _context = context;
            _f = new Fixture();
            _validEmail = "a@a.com";
            _validNI = "NE 01 01 01 C";
            _registrationId = Guid.NewGuid();
            _userIdentityId = Guid.NewGuid();
            _errorItem = new ErrorItem {PropertyName = "", ErrorMessage = "Invalid"};
        }

        [Given(@"the request is valid")]
        public void GivenTheRequestIsValid()
        {
            _command = _f.Build<VerifyIdentityRegistrationCommand>()
                .With(p => p.Email, _validEmail)
                .With(p => p.NationalInsuranceNumber, _validNI)
                .With(p => p.RegistrationId, _registrationId)
                .With(p => p.UserIdentityId, _userIdentityId)
                .Create();
        }

        [Given(@"the inner api will verify registration successfully")]
        public void GivenTheInnerApiWillVerifyRegistrationSuccessfully()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [Given(@"the inner api will not verify registration successfully")]
        public void GivenTheInnerApiWillNotVerifyRegistrationSuccessfully()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.BadRequest)
                        .WithBodyAsJson(new List<ErrorItem>{ _errorItem })
                );
        }

        [When(@"we confirm the identity and verify registration")]
        public Task WhenWeConfirmTheIdentityAndVerifyRegistration()
        {
            return _context.OuterApiClient.Post("/registrations", _command);
        }

        [Then(@"return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"we receive a bad response from the inner api")]
        public void ThenWeReceiveABadResponseFromTheInnerApi()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"call to inner api mapped fields correctly")]
        public void ThenCallToInnerApiMappedFieldsCorrectly()
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<VerifyRegistrationRequestData>(logs.First().RequestMessage.Body);

            innerApiRequest.Should().NotBeNull();
            innerApiRequest.RegistrationId.Should().Be(_command.RegistrationId);
            innerApiRequest.Email.Should().Be(_command.Email);
            innerApiRequest.DateOfBirth.Should().Be(_command.DateOfBirth);
            innerApiRequest.FirstName.Should().Be(_command.FirstName);
            innerApiRequest.LastName.Should().Be(_command.LastName);
            innerApiRequest.UserIdentityId.Should().Be(_command.UserIdentityId);
        }

        [Then(@"the inner api error response is returned")]
        public async Task ThenTheInnerApiErrorResponseIsReturned()
        {
            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ErrorItem>>(content);
            errors.Count.Should().Be(1);
            errors[0].PropertyName.Should().Be(_errorItem.PropertyName);
            errors[0].ErrorMessage.Should().Be(_errorItem.ErrorMessage);
        }
    }
}