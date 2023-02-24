using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Queries.GetProviderAcademicYearEarnings;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Queries.GetProviderAcademicYearEarnings
{
    public class WhenHandlingGetProviderAcademicYearEarningsQuery
    {
        private Fixture _fixture;
        
        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Earnings_Are_Returned(
            GetProviderAcademicYearEarningsQuery query,
            [Frozen] Mock<IFundingProviderEarningsService> earningsService,
            [Frozen] Mock<IApprenticeshipsService> apprenticeshipsService,
            GetProviderAcademicYearEarningsHandler handler
            )
        {
            var learner1 = _fixture.Build<LearnerDto>().With(x => x.Uln, "1001").Create();
            var learner2 = _fixture.Build<LearnerDto>().With(x => x.Uln, "1002").Create();
            var learner3 = _fixture.Build<LearnerDto>().With(x => x.Uln, "1003").Create();
            var earningsResponse = new AcademicYearEarningsDto() { Learners = new List<LearnerDto>() { learner1, learner2, learner3 } };

            var apprentice1 = _fixture.Build<ApprenticeshipDto>().With(x => x.Uln, "1001").Create();
            var apprentice2 = _fixture.Build<ApprenticeshipDto>().With(x => x.Uln, "1002").Create();
            var apprentice3 = _fixture.Build<ApprenticeshipDto>().With(x => x.Uln, "6666").Create();
            var apprenticeshipsResponse = new ApprenticeshipsDto() { Apprenticeships = new List<ApprenticeshipDto>() { apprentice1, apprentice2, apprentice3 } };

            earningsService.Setup(x => x.GetAcademicYearEarnings(query.Ukprn)).ReturnsAsync(earningsResponse);
            apprenticeshipsService.Setup(x => x.GetAll(query.Ukprn)).ReturnsAsync(apprenticeshipsResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.AcademicYearEarnings.Learners.Count.Should().Be(2);
            actual.AcademicYearEarnings.Learners.Find(x => x.Uln == learner1.Uln).Should().BeEquivalentTo(learner1, opts => opts.ExcludingMissingMembers());
            actual.AcademicYearEarnings.Learners.Find(x => x.Uln == learner2.Uln).Should().BeEquivalentTo(learner2, opts => opts.ExcludingMissingMembers());
            actual.AcademicYearEarnings.Learners.Find(x => x.Uln == learner1.Uln).Should().BeEquivalentTo(apprentice1, opts => opts.ExcludingMissingMembers());
            actual.AcademicYearEarnings.Learners.Find(x => x.Uln == learner2.Uln).Should().BeEquivalentTo(apprentice2, opts => opts.ExcludingMissingMembers());
        }
    }
}