﻿using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class ReceiverApplicationApprovedEmailCommand : IRequest<Unit>
    { 
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public long ReceiverId { get; set; }
        public string EncodedApplicationId { get; set; }
        public string EncodedAccountId { get; set; }
        public string BaseUrl { get; set; }
        public string UnsubscribeNotificationsUrl { get; set; }

    }
}
