using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData
{
    public class AddPriorLearningDataCommand : IRequest
    {
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }

        public int? TrainingTotalHours { get; set; }
        public int? DurationReducedByHours { get; set; }
        public bool? IsDurationReducedByRpl { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? CostBeforeRpl { get; set; }
        public int? PriceReducedBy { get; set; }
    }
}
