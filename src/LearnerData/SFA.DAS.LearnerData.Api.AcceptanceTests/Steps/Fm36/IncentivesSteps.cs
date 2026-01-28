using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Extensions;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.NServiceBus;
using System.Net;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps.Fm36;

[Binding]
public class IncentivesSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    [Given(@"the following price episodes")]
    public void GivenTheFollowingPriceEpisodes(Table table)
    {
        var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
        var priceEpisodes = table.CreateSet<PriceEpisodeModel>().ToList();
        apprenticeship.PriceEpisodes = priceEpisodes;
    }

    [Given(@"the following instalments:")]
    public void GivenTheFollowingInstalments(Table table)
    {
        var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
        var instalments = table.CreateSet<InstalmentModel>().ToList();
        apprenticeship.Instalments = instalments;
    }

    [Given(@"the following additional payments:")]
    public void GivenTheFollowingAdditionalPayments(Table table)
    {
        var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
        var additionalPayments = table.CreateSet<AdditionalPaymentModel>().ToList();
        apprenticeship.AdditionalPayments = additionalPayments;
    }

    [When(@"the FM36 block is retrieved for Academic Year (.*) Delivery Period (.*)")]
    public async Task WhenTheFMBlockIsRetrievedForAcademicYearDeliveryPeriod(int academicYear, int deliveryPeriod)
    {
        var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
        var apiResponses = apprenticeship.GetInnerApiResponses();

        testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create().WithPath($"/10005077/{academicYear}/{deliveryPeriod}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(apiResponses.UnPagedLearningsInnerApiResponse)
            );


        testContext.EarningsApi.MockServer
            .Given(
                Request.Create().WithPath($"/{10005077}/fm36/{academicYear}/{deliveryPeriod}")
                    .UsingPost())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(apiResponses.EarningsInnerApiResponse)
            );

        var cancellationToken = new CancellationToken();
        foreach (var sldData in apiResponses.GetSldData())
        {
            await testContext.Cache.StoreLearner(sldData, 10005077, cancellationToken);
        }


        var response = await testContext.OuterApiClient.GetAsync($"/learners/providers/10005077/collectionPeriod/{academicYear}/{deliveryPeriod}/fm36Data");
        var contentString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            NUnit.Framework.TestContext.WriteLine($"Outer api GET fm36 failed: {response.StatusCode}");
            NUnit.Framework.TestContext.WriteLine($"Response body: {contentString}");
        }

        var generatedFm36Learners = JsonConvert.DeserializeObject<List<FM36Learner>>(contentString).ToList();
        scenarioContext.Set(generatedFm36Learners);
    }
}
