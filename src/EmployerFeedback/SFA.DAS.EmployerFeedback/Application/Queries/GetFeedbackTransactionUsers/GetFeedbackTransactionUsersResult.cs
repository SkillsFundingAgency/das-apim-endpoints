using System.Collections.Generic;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers
{
    public class GetFeedbackTransactionUsersResult
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public string TemplateName { get; set; }
        public IEnumerable<FeedbackTransactionUser> Users { get; set; }
    }
}