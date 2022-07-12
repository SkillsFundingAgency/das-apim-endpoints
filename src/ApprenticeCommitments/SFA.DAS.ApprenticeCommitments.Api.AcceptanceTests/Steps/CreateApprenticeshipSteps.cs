using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "CreateApprenticeship")]
    public class CreateApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Apprentice _apprentice;
        private Guid _registrationId = Guid.NewGuid();

        public CreateApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _context.InnerApi.MockServer
                .Given(Request.Create().UsingAnyMethod().WithPath("*"))
                .RespondWith(Response.Create().WithSuccess());
        }


        [Given("an account")]
        public void GivenAnAccount()
        {
            _apprentice = _fixture.Create<Apprentice>();

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*")
                        .UsingGet()
                      )
                .RespondWith(
                    Response.Create()
                        .WithBodyAsJson(_apprentice)
                            );
        }

        [When("the account is matched to an approval")]
        public async Task WhenTheAccountIsMatchedToAnApprovalsync()
        {
            var data = new CreateApprenticeshipFromRegistration.Command()
            {
                RegistrationId = _registrationId,
                ApprenticeId = _apprentice.ApprenticeId,
            };
            await _context.OuterApiClient.Post("apprenticeships", data);
        }

        [Then("the match succeeds")]
        public void ThenTheMatchSucceeds()
        {
            _context.OuterApiClient.Response.Should().Be2XXSuccessful();
        }

        [Then("the account details are passed to the API")]
        public void ThenTheAccountDetailsArePassedToTheAPI()
        {
            _context.InnerApi.MockServer.LogEntries.Should().NotBeNullOrEmpty();
            _context.InnerApi.MockServer.LogEntries.Skip(1).First().RequestMessage.Body.Should().NotBeEmpty()
            //_context.InnerApi.SingleLogBody.Should().NotBeEmpty()
                .And.ShouldBeJson<PostData>()
                .Which.Should().BeEquivalentTo(new
                {
                    RegistrationId = _registrationId,
                    _apprentice.ApprenticeId,
                    _apprentice.LastName,
                    _apprentice.DateOfBirth,
                });
        }

        public class PostData
        {
            public Guid RegistrationId { get; set; }
            public Guid ApprenticeId { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}
