using AutoFixture;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Extensions;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using System.Net;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps.Fm36;

[Binding]
public class MissingEarningsSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private const string _fm36ResponseStatusCodeKey = "Fm36ResponseStatusCode";
    private const int _ukprn = 10005077;
    private const short _academicYear = 2425;
    private const string _deliveryPeriod = "6";
    private readonly Fixture _fixture = new();

    [Given(@"there is data in learnings domain but no data in earnings domain")]
    public void GivenThereIsDataInLearningsDomainButNoDataInEarningsDomain()
    {
        var learnerResponses = new List<Learning>();
        var earningsReponse = new GetFm36DataResponse { Apprenticeships = new List<Apprenticeship>() };

        var apprenticeshipModel = GetApprenticeshipModel();
        var apiResponses = apprenticeshipModel.GetInnerApiResponses();

        learnerResponses.Add(apiResponses.UnPagedLearningsInnerApiResponse.Single());

        testContext.EarningsApi.MockServer
            .Given(
                Request.Create().WithPath($"/{_ukprn}/fm36/{_academicYear}/{_deliveryPeriod}")
                    .UsingPost())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(earningsReponse)
            );

        testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create().WithPath($"/{_ukprn}/{_academicYear}/{_deliveryPeriod}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                .WithBodyAsJson(learnerResponses)
            );
    }

    [When(@"the FM(.*) block is retrieved for the mismatched learner")]
    public async Task WhenTheFMBlockIsRetrievedForTheMismatchedLearner(int p0)
    {
        var response = await testContext.OuterApiClient.GetAsync($"/learners/providers/{_ukprn}/collectionPeriod/{_academicYear}/{_deliveryPeriod}/fm36Data");
        scenarioContext.Set(response.StatusCode, _fm36ResponseStatusCodeKey);
    }

    [Then(@"the response should be a 400 Bad Request")]
    public void ThenTheResponseShouldBeA400BadRequest()
    {
        var statusCode = scenarioContext.Get<HttpStatusCode>(_fm36ResponseStatusCodeKey);
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // gets a valid apprenticeship model with price episodes, instalments and additional payments
    // the content of the models is not important for paging tests
    private ApprenticeshipModel GetApprenticeshipModel()
    {
        var priceEpisode = new PriceEpisodeModel
        {
            Key = _fixture.Create<Guid>(),
            PriceEpisodeId = 1,
            StartDate = DateTime.Now.AddMonths(-6),
            EndDate = DateTime.Now.AddMonths(6)
        };

        var instalments = new List<InstalmentModel>();
        for (var i = 0; i < 12; i++)
        {
            var instalment = new InstalmentModel
            {
                PriceEpisodeId = 1,
                AcademicYear = _academicYear,
                DeliveryPeriod = (byte)(i + 1),
                Amount = 600m,
                InstalmentType = "Regular"
            };

            instalments.Add(instalment);
        }

        return new ApprenticeshipModel
        {
            PriceEpisodes = new List<PriceEpisodeModel> { priceEpisode },
            AdditionalPayments = new List<AdditionalPaymentModel>(),
            Instalments = instalments
        };
    }
}
