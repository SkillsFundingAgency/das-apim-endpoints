using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class GetPendingApplicationEmailDataQueryResult
    {
        public List<EmailData> EmailDataList { get; set; }

        public class EmailData
        {
            public string RecipientEmailAddress { get; set; }
            public string EmployerName { get; set; }
            public int NumberOfApplications { get; set; }
            public long AccountId { get; set; }
        }
    }
}
