using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApproval;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System;
using System.Globalization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ChangeApproval")]
    public class ChangeApprovalSteps
    {
        private readonly TestContext _context;
        private UpdateApprovalCommand _request;
        private IEnumerable<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse> _approvedApprenticeships;
        private IEnumerable<TrainingProviderResponse> _trainingProviderResponses;
        private IEnumerable<Apis.Courses.StandardResponse> _courseResponses;

        public ChangeApprovalSteps(TestContext context)
        {
            _context = context;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/approvals")
                        .UsingPut()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
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
                var apiResponse = new StandardApiResponse
                {
                    Level = course.Level,
                    Title = course.Title,
                    ApprenticeshipFunding = new List<SharedOuterApi.InnerApi.Responses.ApprenticeshipFunding>
                    {
                        new SharedOuterApi.InnerApi.Responses.ApprenticeshipFunding
                        {
                            Duration = course.CourseDuration,
                            EffectiveFrom = new System.DateTime(2020, 1,1),
                            EffectiveTo = new System.DateTime(2030,1,1)
                        }
                    }
                };

                _context.CoursesInnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/courses/standards/{course.Id}")
                            .UsingGet())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(apiResponse)));
            }
        }

        [When("the following apprenticeship update is posted")]
        public async Task WhenTheFollowingApprenticeshipIsPosted(Table table)
        {
            _request = table.CreateInstance<UpdateApprovalCommand>();
            await _context.OuterApiClient.Put("approvals", _request);
        }

        [Then("the response should be OK")]
        public void ThenTheResponseShouldBeOK()
        {
            _context.OuterApiClient.Response.Should().Be2XXSuccessful();
        }

        [Then("the inner API has received the posted values")]
        public void ThenTheRequestToTheInnerApiWasMappedCorrectly()
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.CommitmentsApprenticeshipId);

            _context.OuterApiClient.Response.Should().Be200Ok();

            _context.InnerApi.SingleLogBody.Should().NotBeEmpty()
                .And.ShouldBeJson<ChangeApprovalRequestData>()
                .Which.Should().BeEquivalentTo(new
                {
                    _request.CommitmentsContinuedApprenticeshipId,
                    _request.CommitmentsApprenticeshipId,
                    _request.CommitmentsApprovedOn,
                    expectedCommitment.CourseName,
                    CourseOption = expectedCommitment.Option,
                    PlannedStartDate = expectedCommitment.StartDate,
                });
        }

        [Then("the inner API will not receive any values")]
        public void ThenTheInnerApiWillNotBeCalled()
        {
            _context.InnerApi.SingleLogBody.Should().BeNull();
        }

        [Then("the Employer should be Legal Entity (.*) named '(.*)'")]
        public void ThenTheEmployerNameShouldBe(long legalEntityId, string employerName)
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.CommitmentsApprenticeshipId);

            _context.InnerApi.SingleLogBody.Should().NotBeEmpty()
                .And.ShouldBeJson<ChangeApprovalRequestData>()
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
                .And.Subject.First().RequestMessage.Body.ShouldBeJson<ChangeApprovalRequestData>()
                .Which.Should().BeEquivalentTo(new
                {
                    TrainingProviderId = provider.Ukprn,
                    TrainingProviderName = trainingProviderName,
                });
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

        [Then(@"the employment end date should be ""(.*)""")]
        public void ThenTheEmploymentEndDateShouldBe(string employmentEndDate)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            DateTime? expecteDate = null;
            if (!string.IsNullOrWhiteSpace(employmentEndDate))
            {
                expecteDate = DateTime.ParseExact(employmentEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.EmploymentEndDate.Should().Be(expecteDate);
        }

        [Then(@"the apprentice name should be '(.*)' '(.*)'")]
        public void ThenTheApprenticeNameShouldBe(string firstName, string lastName)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.FirstName.Should().Be(firstName);
            innerApiRequest.LastName.Should().Be(lastName);
        }

        [Then(@"the apprentice date of Birth should be '(.*)'")]
        public void ThenTheApprenticeDateOfBirthShouldBe(string dob)
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<ApprovalCreatedRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.DateOfBirth.ToString("yyyy-MM-dd").Should().Be(dob);
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
}