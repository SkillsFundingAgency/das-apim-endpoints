using AutoFixture;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    public static class BreaksInLearningTestHelper
    {
        public static UpdateLearnerCommand CreateLearnerWithBreaksInLearning(bool withPriceChange)
        {
            var fixture = new Fixture();

            var command = fixture.Create<UpdateLearnerCommand>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Clear();

            var standardCode = fixture.Create<int>();
            var agreementId = fixture.Create<string>();
            var startDate = fixture.Create<DateTime>();
            var pauseDate = startDate.AddMonths(6);
            var resumeDate = pauseDate.AddMonths(6);

            var initialCosts = new List<CostDetails>
            {
                new CostDetails
                {
                    FromDate = startDate,
                    TrainingPrice = fixture.Create<int>(),
                    EpaoPrice = fixture.Create<int>(),
                }
            };

            var resumeCosts = withPriceChange ?
                [
                    new CostDetails
                    {
                        FromDate = resumeDate,
                        TrainingPrice = initialCosts.First().TrainingPrice + 1000,
                        EpaoPrice = initialCosts.First().EpaoPrice + 1000,
                    }
                ]
                : initialCosts;

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddYears(2),
                ActualEndDate = pauseDate,
                StandardCode = standardCode,
                AgreementId = agreementId,
                PauseDate = null,
                Costs = initialCosts,
                LearningSupport = []
            });

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = resumeDate,
                ExpectedEndDate = resumeDate.AddYears(2),
                StandardCode = standardCode,
                AgreementId = agreementId,
                PauseDate = null,
                WithdrawalDate = null,
                CompletionDate = null,
                Costs = resumeCosts,
                LearningSupport = []
            });

            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Clear();

            return command;
        }

        public static UpdateLearnerCommand CreateLearnerWithEnglishAndMathsBreaksInLearning()
        {
            var fixture = new Fixture();

            var command = fixture.Create<UpdateLearnerCommand>();
            var startDate = fixture.Create<DateTime>();
            var pauseDate = startDate.AddMonths(6);

            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Clear();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Clear();

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddYears(2),
                ActualEndDate = pauseDate,
                PauseDate = null,
                LearningSupport = []
            });

            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Add(new MathsAndEnglish
            {
                StartDate = startDate,
                EndDate = startDate.AddYears(2),
                PauseDate = pauseDate,
                LearningSupport = []
            });

            return command;
        }
    }
}
