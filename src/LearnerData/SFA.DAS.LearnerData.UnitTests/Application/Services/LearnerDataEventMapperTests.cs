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
            var uln = _fixture.Create<long>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var email = _fixture.Create<string>();
            var dob = _fixture.Create<DateTime>();
            var onProgramme = _fixture.Create<OnProgrammeRequestDetails>();
            var learningType = LearningType.Apprenticeship;
            var correlationId = _fixture.Create<Guid>();
            var receivedDate = _fixture.Create<DateTime>();
            var consumerReference = _fixture.Create<string>();

            // Act
            var result = _sut.Build(ukprn, uln, firstName, lastName, email, dob, onProgramme, learningType, correlationId, receivedDate, consumerReference);

            // Assert
            result.Should().BeEquivalentTo(new
            {
                ULN = uln,
                UKPRN = ukprn,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DoB = dob,
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

            // Act
            var result = _sut.Build(1, 1, "First", "Last", null, DateTime.Today, onProgramme, LearningType.Apprenticeship, Guid.NewGuid(), DateTime.Today, null);

            // Assert
            result.TrainingPrice.Should().Be(0);
            result.EpaoPrice.Should().Be(0);
        }
    }
}
