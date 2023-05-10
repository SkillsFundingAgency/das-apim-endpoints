using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData
{
    public class AddPriorLearningDataCommandResult
    {
        public bool HasStandardOptions { get; set; }
        public bool RplPriceReductionError { get; set; }

    }
}
