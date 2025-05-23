using System.Net;
using System.Net.Http.Json;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using SFA.DAS.Earnings.Api.AcceptanceTests.Extensions;
using SFA.DAS.Earnings.Api.AcceptanceTests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Steps
{
    [Binding]
    public class IncentivesSteps(TestContext testContext)
    {
        private readonly TestContext _testContext = testContext;
        private HttpResponseMessage _response;
        private List<FM36Learner> _generatedFm36Learners;

        [Given(@"an apprentice aged (.*) at the start of the apprenticeship")]
        public void GivenAnApprenticeAgedAtTheStartOfTheApprenticeship(int age)
        {
            var apprenticeship = testContext.Apprenticeship;

            apprenticeship.Age = age;


        }

        [Given(@"the following price episodes")]
        public void GivenTheFollowingPriceEpisodes(Table table)
        {
            var apprenticeship = testContext.Apprenticeship;
            var priceEpisodes = table.CreateSet<Models.PriceEpisode>().ToList();
            apprenticeship.PriceEpisodes = priceEpisodes;
        }

        [Given(@"the following instalments:")]
        public void GivenTheFollowingInstalments(Table table)
        {
            var apprenticeship = testContext.Apprenticeship;
            var instalments = table.CreateSet<Instalment>().ToList();
            apprenticeship.Instalments = instalments;
        }

        [Given(@"the following additional payments:")]
        public void GivenTheFollowingAdditionalPayments(Table table)
        {
            var apprenticeship = testContext.Apprenticeship;
            var additionalPayments = table.CreateSet<AdditionalPayment>().ToList();
            apprenticeship.AdditionalPayments = additionalPayments;
        }

        [When(@"the FM36 block is retrieved for Academic Year (.*) Delivery Period (.*)")]
        public async Task WhenTheFMBlockIsRetrievedForAcademicYearDeliveryPeriod(int academicYear, int deliveryPeriod)
        {
            var apprenticeship = testContext.Apprenticeship;
            var apiResponses = apprenticeship.GetInnerApiResponses();

            testContext.ApprenticeshipsApi.MockServer
                .Given(
                    Request.Create().WithPath($"/10005077/{academicYear}/{deliveryPeriod}")
                        .UsingGet())
                .RespondWith(
            Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBodyAsJson(apiResponses.ApprenticeshipsInnerApiResponse)
                );

            testContext.EarningsApi.MockServer
                .Given(
                    Request.Create().WithPath($"/{10005077}/fm36/{academicYear}/{deliveryPeriod}")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBodyAsJson(apiResponses.EarningsInnerApiResponse)
                );

            _response = await _testContext.OuterApiClient.GetAsync($"/learners/10005077/{academicYear}/{deliveryPeriod}");
            var contentString = await _response.Content.ReadAsStringAsync();

            if (!_response.IsSuccessStatusCode)
            {
                NUnit.Framework.TestContext.WriteLine($"Outer api GET fm36 failed: {_response.StatusCode}");
                NUnit.Framework.TestContext.WriteLine($"Response body: {contentString}");
            }
            
            _generatedFm36Learners = JsonConvert.DeserializeObject<List<FM36Learner>>(contentString).ToList();

        }

        [Then(@"the Price Episode Periodised Values are as follows:")]
        public void ThenThePriceEpisodePeriodisedValuesAreAsFollows(Table table)
        {
            var expected = table.CreateSet<Models.PriceEpisodePeriodisedValues>().ToList();

            var learner = _generatedFm36Learners.First();

            foreach (var expectation in expected)
            {
                var priceEpisode = learner.PriceEpisodes[expectation.Episode];

                var actual =
                    priceEpisode.PriceEpisodePeriodisedValues.Single(x => x.AttributeName == expectation.Attribute);

                actual.GetPeriodValue(expectation.Period).Should().Be(expectation.Value);
            }
        }
    }
}
