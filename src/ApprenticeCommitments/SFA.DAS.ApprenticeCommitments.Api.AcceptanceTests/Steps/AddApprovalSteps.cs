using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApproval;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "AddApproval")]
    public class AddApprovalSteps
    {
        private readonly TestContext _context;
        private CreateApprovalCommand _request;
        private IEnumerable<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse> _approvedApprenticeships;
        private IEnumerable<TrainingProviderResponse> _trainingProviderResponses;
        private IEnumerable<Apis.Courses.StandardResponse> _courseResponses;

        public AddApprovalSteps(TestContext context)
        {
            _context = context;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/approvals")
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
                    Request.Create().WithPath("/approvals")
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
            _trainingProviderResponses = table.CreateSet<TrainingProviderResponse>();

            foreach (var trainingProvider in _trainingProviderResponses)
            {
                _context.TrainingProviderInnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/v1/search")
                            .WithParam("searchterm", true,$"{trainingProvider.Ukprn}")
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(new SearchResponse { SearchResults = new [] { trainingProvider }}))
                    );
            }
        }

        [Given("the following courses exist")]
        public void GivenTheFollowingCoursesExist(Table table)
        {
            _courseResponses = table.CreateSet<Apis.Courses.StandardResponse>();

            foreach (var course in _courseResponses)
            {
                var id = string.IsNullOrWhiteSpace(course.StandardUId)
                    ? course.Id.ToString()
                    : course.StandardUId;

                var apiResponse = new StandardApiResponse
                {
                    Level = course.Level,
                    Title = course.Title,
                    ApprenticeshipFunding = new List<SharedOuterApi.InnerApi.Responses.ApprenticeshipFunding>
                    {
                        new SharedOuterApi.InnerApi.Responses.ApprenticeshipFunding
                        {
                            Duration = course.CourseDuration,
                            EffectiveFrom = new DateTime(2020, 1,1),
                            EffectiveTo = new DateTime(2030,1,1)
                        }
                    }
                };

                _context.CoursesInnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/courses/standards/{id}")
                            .UsingGet())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(apiResponse)));
            }
        }

        [When("the following apprenticeship is posted")]
        public async Task WhenTheFollowingApprenticeshipIsPosted(Table table)
        {
            _request = table.CreateInstance<CreateApprovalCommand>();
            await _context.OuterApiClient.Post("approvals", _request);
        }

        [Then("the inner API has received the posted values")]
        public void ThenTheRequestToTheInnerApiWasMappedCorrectly()
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.CommitmentsApprenticeshipId);

            _context.OuterApiClient.Response.Should().Be200Ok();

            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.Should().NotBeNull();
            innerApiRequest.RegistrationId.Should().NotBe(Guid.Empty);
            innerApiRequest.FirstName.Should().Be(expectedCommitment.FirstName);
            innerApiRequest.LastName.Should().Be(expectedCommitment.LastName);
            innerApiRequest.DateOfBirth.Should().Be(expectedCommitment.DateOfBirth);
            innerApiRequest.Email.Should().Be(expectedCommitment.Email);
            innerApiRequest.CommitmentsApprenticeshipId.Should().Be(_request.CommitmentsApprenticeshipId);
            innerApiRequest.CommitmentsApprovedOn.Should().Be(_request.CommitmentsApprovedOn);
            innerApiRequest.EmployerName.Should().Be(_request.EmployerName);
            innerApiRequest.EmployerAccountLegalEntityId.Should().Be(_request.EmployerAccountLegalEntityId);
            innerApiRequest.TrainingProviderId.Should().Be(_request.TrainingProviderId);
            innerApiRequest.PlannedStartDate.Should().Be(expectedCommitment.StartDate);
            innerApiRequest.EmploymentEndDate.Should().Be(expectedCommitment.EmploymentEndDate);
            innerApiRequest.CourseOption.Should().Be(expectedCommitment.Option);
        }

        [Then("the Training Provider Name should be '(.*)'")]
        public void ThenTheTrainingProviderNameShouldBe(string trainingProviderName)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.TrainingProviderName.Should().Be(trainingProviderName);
        }

        [Then("recognise prior learning should be (.*) with total hours reduction as (.*) and weeks reduction (.*)")]
        public void ThenTheCourseLevelDurationShouldBe(bool rplApplies, int? hours, int? weeks)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.RecognisePriorLearning.Should().Be(rplApplies);
            innerApiRequest.DurationReducedByHours.Should().Be(hours);
            innerApiRequest.DurationReducedBy.Should().Be(weeks);
        }

        [Then(@"there is no recognised prior learning values")]
        public void ThenThereIsNoRecognisedPriorLearningValues()
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.RecognisePriorLearning.Should().BeFalse();
            innerApiRequest.DurationReducedByHours.Should().BeNull();
            innerApiRequest.DurationReducedBy.Should().BeNull();
        }

        [Then("the course should be `(.*)` level (.*) courseDuration (.*)")]
        public void ThenTheCourseLevelDurationShouldBe(string name, int level, int courseDuration)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.CourseName.Should().Be(name);
            innerApiRequest.CourseLevel.Should().Be(level);
            innerApiRequest.CourseDuration.Should().Be(courseDuration);
        }

        [Then(@"the delivery model should be ""(.*)""")]
        public void ThenTheDeliveryModelShouldBe(string deliveryModel)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.DeliveryModel.Should().Be(Enum.Parse<DeliveryModel>(deliveryModel));
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

        [Then("the request should be ignored")]
        public void ThenTheRequestShouldBeIgnored()
        {
            _context.OuterApiClient.Response.Should().Be2XXSuccessful();
            _context.InnerApi.MockServer.LogEntries.Should().BeEmpty();
        }

        [Then("the invitation was sent successfully")]
        public async Task ThenTheInvitationWasSentSuccessfully()
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.CommitmentsApprenticeshipId);

            var response = JsonConvert.DeserializeObject<CreateApprovalResponse>(
                await _context.OuterApiClient.Response.Content.ReadAsStringAsync());

            response.Should().BeEquivalentTo(new
            {
                GivenName = expectedCommitment.FirstName,
                FamilyName = expectedCommitment.LastName,
                ApprenticeshipName = expectedCommitment.CourseName,
            });
        }

        [Then("the invitation was not sent")]
        public void ThenTheInvitationWasNotSent()
        {
            _context.LoginApi.MockServer.LogEntries.Should().BeEmpty();
        }
    }
}