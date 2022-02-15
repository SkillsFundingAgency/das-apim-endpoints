using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class RejectApplicationsCommand : IRequest
    { 
        public int PledgeId { get; set; }
        public int AccountId { get; set; }     
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public List<int> ApplicationsToReject { get; set; }
    }
}