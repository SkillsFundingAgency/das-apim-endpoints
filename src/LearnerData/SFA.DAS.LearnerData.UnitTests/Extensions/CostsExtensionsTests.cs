using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.UnitTests.Extensions
{
    [TestFixture]
    public class CostsExtensionsTests
    {
        [Test]
        public void Null_Costs_ShouldReturnSingleDefaultCost()
        {
            var startDate = new DateTime(2023, 9, 1);
            var source = new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                Costs = null
            };

            var result = source.Costs.GetCostsOrDefault(source.StartDate);

            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new Cost
                {
                    TrainingPrice = 0,
                    EpaoPrice = null,
                    FromDate = startDate
                });
        }

        [Test]
        public void Empty_Costs_ShouldReturnSingleDefaultCost()
        {
            var startDate = new DateTime(2023, 9, 1);
            var source = new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                Costs = new List<CostDetails>()
            };

            var result = source.Costs.GetCostsOrDefault(source.StartDate);

            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new Cost
                {
                    TrainingPrice = 0,
                    EpaoPrice = null,
                    FromDate = startDate
                });
        }

        [Test]
        public void Costs_ShouldMapEachCostCorrectly()
        {
            var startDate = new DateTime(2023, 9, 1);
            var costDate = new DateTime(2023, 10, 1);
            var source = new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                Costs = new List<CostDetails>
                {
                    new CostDetails { TrainingPrice = 1000, EpaoPrice = 200, FromDate = costDate },
                    new CostDetails { TrainingPrice = null, EpaoPrice = 300, FromDate = null }
                }
            };

            var result = source.Costs.GetCostsOrDefault(source.StartDate);

            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new Cost
            {
                TrainingPrice = 1000,
                EpaoPrice = 200,
                FromDate = costDate
            });

            result[1].Should().BeEquivalentTo(new Cost
            {
                TrainingPrice = 0,
                EpaoPrice = 300,
                FromDate = startDate
            });
        }
    }
}