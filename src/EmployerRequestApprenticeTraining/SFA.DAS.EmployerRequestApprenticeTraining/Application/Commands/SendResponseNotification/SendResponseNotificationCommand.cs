using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification
{
    public class SendResponseNotificationCommand : IRequest<Unit>
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public long AccountId { get; set; }
        public Guid RequestedBy { get; set; }
        public List<StandardDetails> Standards{ get; set; }
        public string EmployerAccountsBaseUrl { get; set; }
        public string EmployerRequestApprenticeshipTrainingBaseUrl { get; set; }
    }
    public class StandardDetails
    {
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
    }
}
