using AutoFixture;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class EarningsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EarningsController> _logger;

    public EarningsController(IMediator mediator, ILogger<EarningsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("{collectionPeriod}/{ukprn}")]
    public async Task<IActionResult> GetEarnings(string collectionPeriod, long ukprn)
    {
        return Ok(GetLearners(ukprn));
    }

    /// <summary>
    /// NOTE: When you remove this block please remove the reference to AutoFixture
    /// </summary>
    /// <param name="ukprn"></param>
    /// <returns></returns>
    private FM36Learner[] GetLearners(long ukprn)
    {
        if (ukprn == 99999999)
        {
            return [];
        }

        Fixture fixture = new Fixture();

        return new[]
        {
            new FM36Learner
            {
                ULN = fixture.Create<long>(),
                LearnRefNumber = Guid.NewGuid().ToString(),
                PriceEpisodes = CreatePriceEpisodes(),
                LearningDeliveries = CreateLearningDeliveries(),
            }
        };
    }

    private List<LearningDelivery> CreateLearningDeliveries()
    {
        return new List<LearningDelivery>
        {
            new LearningDelivery
            {
                AimSeqNumber = 1,
                LearningDeliveryValues = new LearningDeliveryValues
                {
                    AdjStartDate = new DateTime(2024, 08, 01),
                    AgeAtProgStart = 22,
                    AppAdjLearnStartDate = new DateTime(2024, 08, 01),
                    FundStart = true,
                    LearnAimRef = "ZPROG001",
                    LearnStartDate = new DateTime(2024, 08, 01),
                    LearnDel1618AtStart = false,
                    LearnDelAppAccDaysIL = 365,
                    LearnDelHistDaysThisApp = 0,
                    LearnDelHistProgEarnings = 0,
                    LearnDelInitialFundLineType = "19+ Apprenticeship (Employer on App Service)",
                    PlannedNumOnProgInstalm = 12,
                    PlannedTotalDaysIL = 365,
                    ProgType = 25,
                    StdCode = 12345
                },
                LearningDeliveryPeriodisedValues = new List<LearningDeliveryPeriodisedValues>
                {
                    new LearningDeliveryPeriodisedValues { AttributeName = "InstPerPeriod", Period1 = 1, Period2 = 1, Period3 = 1, Period4 = 1, Period5 = 1, Period6 = 1, Period7 = 1, Period8 = 1, Period9 = 1, Period10 = 1, Period11 = 1, Period12 = 1 },
                    new LearningDeliveryPeriodisedValues { AttributeName = "LearnDelESFAContribPct", Period1 = 0.95m, Period2 = 0.95m, Period3 = 0.95m, Period4 = 0.95m, Period5 = 0.95m, Period6 = 0.95m, Period7 = 0.95m, Period8 = 0.95m, Period9 = 0.95m, Period10 = 0.95m, Period11 = 0.95m, Period12 = 0.95m },
                    new LearningDeliveryPeriodisedValues { AttributeName = "ProgrammeAimOnProgPayment", Period1 = 666.67m, Period2 = 666.67m, Period3 = 666.67m, Period4 = 666.67m, Period5 = 666.67m, Period6 = 666.67m, Period7 = 666.67m, Period8 = 666.67m, Period9 = 666.67m, Period10 = 666.67m, Period11 = 666.67m, Period12 = 666.67m },
                    new LearningDeliveryPeriodisedValues { AttributeName = "ProgrammeAimProgFundIndMaxEmpCont", Period1 = 633.34m, Period2 = 633.34m, Period3 = 633.34m, Period4 = 633.34m, Period5 = 633.34m, Period6 = 633.34m, Period7 = 633.34m, Period8 = 633.34m, Period9 = 633.34m, Period10 = 633.34m, Period11 = 633.34m, Period12 = 633.34m },
                    new LearningDeliveryPeriodisedValues { AttributeName = "ProgrammeAimProgFundIndMinCoInvest", Period1 = 33.33m, Period2 = 33.33m, Period3 = 33.33m, Period4 = 33.33m, Period5 = 33.33m, Period6 = 33.33m, Period7 = 33.33m, Period8 = 33.33m, Period9 = 33.33m, Period10 = 33.33m, Period11 = 33.33m, Period12 = 33.33m },
                    new LearningDeliveryPeriodisedValues { AttributeName = "ProgrammeAimTotProgFund", Period1 = 633.34m, Period2 = 633.34m, Period3 = 633.34m, Period4 = 633.34m, Period5 = 633.34m, Period6 = 633.34m, Period7 = 633.34m, Period8 = 633.34m, Period9 = 633.34m, Period10 = 633.34m, Period11 = 633.34m, Period12 = 633.34m },
                },
                LearningDeliveryPeriodisedTextValues = new List<LearningDeliveryPeriodisedTextValues>
                {
                    new LearningDeliveryPeriodisedTextValues { AttributeName = "FundLineType", Period1 = "19+ Apprenticeship (Employer on App Service)", Period2 = "19+ Apprenticeship (Employer on App Service)", Period3 = "19+ Apprenticeship (Employer on App Service)", Period4 = "19+ Apprenticeship (Employer on App Service)", Period5 = "19+ Apprenticeship (Employer on App Service)", Period6 = "19+ Apprenticeship (Employer on App Service)", Period7 = "19+ Apprenticeship (Employer on App Service)", Period8 = "19+ Apprenticeship (Employer on App Service)", Period9 = "19+ Apprenticeship (Employer on App Service)", Period10 = "19+ Apprenticeship (Employer on App Service)", Period11 = "19+ Apprenticeship (Employer on App Service)", Period12 = "19+ Apprenticeship (Employer on App Service)" },
                    new LearningDeliveryPeriodisedTextValues { AttributeName = "LearnDelContType", Period1 = "ACT1", Period2 = "ACT1", Period3 = "ACT1", Period4 = "ACT1", Period5 = "ACT1", Period6 = "ACT1", Period7 = "ACT1", Period8 = "ACT1", Period9 = "ACT1", Period10 = "ACT1", Period11 = "ACT1", Period12 = "ACT1" }
                }
            }
        };
    }

    private List<PriceEpisode> CreatePriceEpisodes()
    {
        return new List<PriceEpisode>()
        {
            new PriceEpisode
            {
                PriceEpisodeIdentifier = "25-12345-2024-08-01",
                PriceEpisodeValues = new PriceEpisodeValues
                {
                    EpisodeStartDate = new DateTime(2024, 08, 01),
                    TNP1 = 8000,
                    TNP2 = 2000,
                    PriceEpisodeUpperBandLimit = 10000,
                    PriceEpisodePlannedEndDate = new DateTime(2025, 07, 31),
                    PriceEpisodeTotalTNPPrice = 10000,
                    PriceEpisodeUpperLimitAdjustment = 0,
                    PriceEpisodePlannedInstalments = 12,
                    PriceEpisodeActualInstalments = 12,
                    PriceEpisodeInstalmentsThisPeriod = 1,
                    PriceEpisodeCompletionElement = 2000,
                    PriceEpisodeInstalmentValue = 666.67m,
                    PriceEpisodeOnProgPayment = 666.67m,
                    PriceEpisodeTotalEarnings = 666.67m,
                    PriceEpisodeCompleted = false,
                    PriceEpisodeRemainingTNPAmount = 10000,
                    PriceEpisodeRemainingAmountWithinUpperLimit = 10000,
                    PriceEpisodeCappedRemainingTNPAmount = 10000,
                    PriceEpisodeExpectedTotalMonthlyValue = 8000,
                    PriceEpisodeAimSeqNumber = 1,
                    PriceEpisodeFundLineType = "19+ Apprenticeship (Employer on App Service)",
                    EpisodeEffectiveTNPStartDate = new DateTime(2024, 08, 01),
                    PriceEpisodeContractType = "ACT1",
                    PriceEpisodePreviousEarningsSameProvider = 0,
                    PriceEpisodeTotProgFunding = 8000,
                    PriceEpisodeProgFundIndMinCoInvest = 7600,
                    PriceEpisodeProgFundIndMaxEmpCont = 1400,
                    PriceEpisodeLDAppIdent = "25-12345"
                },
                PriceEpisodePeriodisedValues = new List<PriceEpisodePeriodisedValues>
                {
                    new PriceEpisodePeriodisedValues { AttributeName = "PriceEpisodeInstalmentsThisPeriod", Period1 = 1, Period2 = 1, Period3 = 1, Period4 = 1, Period5 = 1, Period6 = 1, Period7 = 1, Period8 = 1, Period9 = 1, Period10 = 1, Period11 = 1, Period12 = 1 },
                    new PriceEpisodePeriodisedValues { AttributeName = "PriceEpisodeOnProgPayment", Period1 = 666.67m, Period2 = 666.67m, Period3 = 666.67m, Period4 = 666.67m, Period5 = 666.67m, Period6 = 666.67m, Period7 = 666.67m, Period8 = 666.67m, Period9 = 666.67m, Period10 = 666.67m, Period11 = 666.67m, Period12 = 666.67m },
                    new PriceEpisodePeriodisedValues { AttributeName = "PriceEpisodeProgFundIndMaxEmpCont", Period1 = 633.34m, Period2 = 633.34m, Period3 = 633.34m, Period4 = 633.34m, Period5 = 633.34m, Period6 = 633.34m, Period7 = 633.34m, Period8 = 633.34m, Period9 = 633.34m, Period10 = 633.34m, Period11 = 633.34m, Period12 = 633.34m },
                    new PriceEpisodePeriodisedValues { AttributeName = "PriceEpisodeProgFundIndMinCoInvest", Period1 = 33.33m, Period2 = 33.33m, Period3 = 33.33m, Period4 = 33.33m, Period5 = 33.33m, Period6 = 33.33m, Period7 = 33.33m, Period8 = 33.33m, Period9 = 33.33m, Period10 = 33.33m, Period11 = 33.33m, Period12 = 33.33m },
                    new PriceEpisodePeriodisedValues { AttributeName = "PriceEpisodeESFAContribPct", Period1 = 0.95m, Period2 = 0.95m, Period3 = 0.95m, Period4 = 0.95m, Period5 = 0.95m, Period6 = 0.95m, Period7 = 0.95m, Period8 = 0.95m, Period9 = 0.95m, Period10 = 0.95m, Period11 = 0.95m, Period12 = 0.95m },
                    new PriceEpisodePeriodisedValues { AttributeName = "PriceEpisodeTotProgFunding", Period1 = 633.34m, Period2 = 633.34m, Period3 = 633.34m, Period4 = 633.34m, Period5 = 633.34m, Period6 = 633.34m, Period7 = 633.34m, Period8 = 633.34m, Period9 = 633.34m, Period10 = 633.34m, Period11 = 633.34m, Period12 = 633.34m },
                }
            }
        };
    }
}