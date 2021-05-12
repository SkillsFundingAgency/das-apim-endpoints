using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ChangeApprenticeship")]
    public class ChangeApprenticeshipSteps
    {
        private readonly TestContext _context;
        private UpdateApprenticeshipCommand _request;
        private IEnumerable<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse> _approvedApprenticeships;
        private IEnumerable<Apis.TrainingProviderApi.TrainingProviderResponse> _trainingProviderResponses;
        private IEnumerable<Apis.Courses.StandardResponse> _courseResponses;

        public ChangeApprenticeshipSteps(TestContext context)
        {
            _context = context;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprenticeships/change")
                        .UsingPost()
                        .WithBody(new JmesPathMatcher(
                            "ApprenticeshipId != `0` && contains(Email, '@')"))
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                            );

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/apprenticeships/change")
                        .UsingPost()
                        .WithBody(new JmesPathMatcher(
                            "!contains(Email, '@')"))
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.BadRequest)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{'email':'Not valid'}")
                            );

            _context.LoginApi.MockServer
                .Given(
                    Request.Create().WithPath($"/invitations/{_context.LoginConfig.IdentityServerClientId}")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                            );
        }

        [Given("the following apprenticeships have been approved")]
        public void GivenTheFollowingApprenticeshipsHaveBeenApproved(Table table)
        {
            _approvedApprenticeships = table.CreateSet<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>();

            foreach (var approval in _approvedApprenticeships)
            {
                _context.CommitmentsV2InnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/apprenticeships/{approval.Id}")
                            .UsingGet()
                          )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(approval))
                                );
            }
        }

        [Given("the following training providers exist")]
        public void GivenTheFollowingTrainingProvidersExist(Table table)
        {
            _trainingProviderResponses = table.CreateSet<Apis.TrainingProviderApi.TrainingProviderResponse>();

            foreach (var trainingProvider in _trainingProviderResponses)
            {
                _context.TrainingProviderInnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/v1/search")
                            .WithParam("searchterm", true, $"{trainingProvider.Ukprn}")
                            .UsingGet()
                          )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(new SearchResponse { SearchResults = new[] { trainingProvider } }))
                                );
            }
        }

        [Given("the following courses exist")]
        public void GivenTheFollowingCoursesExist(Table table)
        {
            _courseResponses = table.CreateSet<Apis.Courses.StandardResponse>();

            foreach (var course in _courseResponses)
            {
                _context.CoursesInnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/courses/standards/{course.Id}")
                            .UsingGet())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(course)));
            }
        }

        [When("the following apprenticeship update is posted")]
        public async Task WhenTheFollowingApprenticeshipIsPosted(Table table)
        {
            _request = table.CreateInstance<UpdateApprenticeshipCommand>();
            await _context.OuterApiClient.Post("apprenticeships/update", _request);
        }

        [Then("the inner API has received the posted values")]
        public void ThenTheRequestToTheInnerApiWasMappedCorrectly()
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.ApprenticeshipId);

            _context.OuterApiClient.Response.StatusCode
                .Should().Be(HttpStatusCode.Accepted);

            _context.InnerApi.SingleLogBody.Should().NotBeEmpty()
                .And.ShouldBeJson<ChangeApprenticeshipRequestData>()
                .Which.Should().BeEquivalentTo(new
                {
                    _request.ApprenticeshipId,
                    _request.Email,
                    ApprovedOn = _request.ApprovedOn,
                    expectedCommitment.CourseName,
                    PlannedStartDate = expectedCommitment.StartDate,
                });
        }

        [Then("the Employer should be Legal Entity (.*) named '(.*)'")]
        public void ThenTheEmployerNameShouldBe(long legalEntityId, string employerName)
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.ApprenticeshipId);

            _context.InnerApi.SingleLogBody.Should().NotBeEmpty()
                .And.ShouldBeJson<ChangeApprenticeshipRequestData>()
                .Which.Should().BeEquivalentTo(new
                {
                    EmployerAccountLegalEntityId = expectedCommitment.AccountLegalEntityId,
                    expectedCommitment.EmployerName,
                });
        }

        [Then("the Training Provider should be '(.*)'")]
        public void ThenTheTrainingProviderNameShouldBe(string trainingProviderName)
        {
            var provider = _trainingProviderResponses.First(x =>
                x.LegalName == trainingProviderName || x.TradingName == trainingProviderName);

            _context.InnerApi.MockServer.LogEntries.Should().NotBeEmpty()
                .And.Subject.First().RequestMessage.Body.ShouldBeJson<ChangeApprenticeshipRequestData>()
                .Which.Should().BeEquivalentTo(new
                {
                    TrainingProviderId = provider.Ukprn,
                    TrainingProviderName = trainingProviderName,
                });
        }

        [Then("the course should be `(.*)` level (.*)")]
        public void ThenTheCourseLevelShouldBe(string name, int level)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<CreateApprenticeshipRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.CourseName.Should().Be(name);
            innerApiRequest.CourseLevel.Should().Be(level);
        }

        [Then("the inner API should return these errors")]
        public async Task ThenTheInnerAPIShouldReturnTheseErrors(Table table)
        {
            _context.OuterApiClient.Response.StatusCode
                .Should().Be(HttpStatusCode.BadRequest);

            var expectedErrors = table.Rows.ToDictionary(r => r[0], r => r[1]);

            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            errors.Should().BeEquivalentTo(expectedErrors);
        }
    }

    public static class JsonConvertExtensions
    {
        public static AndWhichConstraint<JsonConvertAssertions, T> ShouldBeJson<T>(this string instance)
        {
            return new JsonConvertAssertions(instance).DeserialiseTo<T>();
        }

        public static AndWhichConstraint<JsonConvertAssertions, T> ShouldBeJson<T>(this StringAssertions instance)
        {
            return new JsonConvertAssertions(instance.Subject).DeserialiseTo<T>();
        }

        public class JsonConvertAssertions :
            ReferenceTypeAssertions<string, JsonConvertAssertions>
        {
            public JsonConvertAssertions(string instance)
            {
                Subject = instance;
            }

            protected override string Identifier => "string";

            public AndWhichConstraint<JsonConvertAssertions, T> DeserialiseTo<T>(
                string because = "", params object[] becauseArgs)
            {
                var deserialised = JsonConvert.DeserializeObject<T>(Subject);

                Execute.Assertion
                    .ForCondition(deserialised != null)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} to contain deserilise to {0}{reason}, but found.",
                        typeof(T).Name);

                return new AndWhichConstraint<JsonConvertAssertions, T>(this, deserialised);
            }
        }
    }
}