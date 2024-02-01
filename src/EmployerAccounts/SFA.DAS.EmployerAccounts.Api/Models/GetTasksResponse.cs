using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetTasksResponse
    {
        public bool ShowLevyDeclarationTask { get; set; }
        public int NumberOfApprenticesToReview { get; set; }
        public int NumberOfCohortsReadyToReview { get; set; }
        public int NumberOfPendingTransferConnections { get; set; }
        public int NumberOfTransferRequestToReview { get; set; }
        public int NumberTransferPledgeApplicationsToReview { get; set; }

        public static implicit operator GetTasksResponse(GetTasksQueryResult source)
        {
            return new GetTasksResponse
            {
                ShowLevyDeclarationTask = source.ShowLevyDeclarationTask,
                NumberOfApprenticesToReview = source.NumberOfApprenticesToReview,
                NumberOfCohortsReadyToReview = source.NumberOfCohortsReadyToReview,
                NumberOfPendingTransferConnections = source.NumberOfPendingTransferConnections,
                NumberOfTransferRequestToReview = source.NumberOfTransferRequestToReview,
                NumberTransferPledgeApplicationsToReview = source.NumberTransferPledgeApplicationsToReview
            };
        }
    }
}