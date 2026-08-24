using AutoFixture;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class LearnerDataEventMapperTests
    {
        private readonly Fixture _fixture = new();
        private readonly LearnerDataEventMapper _sut = new();

        [Test]
        public void Then_maps_all_fields_onto_the_event()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var learner = _fixture.Create<LearnerRequestDetails>();
            var onProgramme = _fixture.Create<OnProgrammeRequestDetails>();
            var learningType = LearningType.Apprenticeship;
            var correlationId = _fixture.Create<Guid>();
            var receivedDate = _fixture.Create<DateTime>();
            var consumerReference = _fixture.Create<string>();

            // Act
            var result = _sut.Build(ukprn, learner, onProgramme, learningType, correlationId, receivedDate, consumerReference);

            // Assert
            result.Should().BeEquivalentTo(new
            {
                ULN = learner.Uln,
                UKPRN = ukprn,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Email = learner.Email,
                DoB = learner.Dob,
                StartDate = onProgramme.StartDate,
                PlannedEndDate = onProgramme.ExpectedEndDate,
                PercentageLearningToBeDelivered = onProgramme.PercentageOfTrainingLeft,
                EpaoPrice = onProgramme.Costs!.First().EpaoPrice,
                TrainingPrice = onProgramme.Costs!.First().TrainingPrice,
                AgreementId = onProgramme.AgreementId,
                IsFlexiJob = onProgramme.IsFlexiJob!.Value,
                StandardCode = onProgramme.StandardCode,
                CorrelationId = correlationId,
                ReceivedDate = receivedDate,
                ConsumerReference = consumerReference,
                LearningType = learningType
            });
        }

        [Test]
        public void Then_defaults_prices_when_no_costs_are_supplied()
        {
            // Arrange
            var onProgramme = _fixture.Build<OnProgrammeRequestDetails>()
                .With(x => x.Costs, (List<CostDetails>?)null)
                .Create();
            var learner = _fixture.Create<LearnerRequestDetails>();

            // Act
            var result = _sut.Build(1, learner, onProgramme, LearningType.Apprenticeship, Guid.NewGuid(), DateTime.Today, null);

            // Assert
            result.TrainingPrice.Should().Be(0);
            result.EpaoPrice.Should().Be(0);
        }
    }
}
