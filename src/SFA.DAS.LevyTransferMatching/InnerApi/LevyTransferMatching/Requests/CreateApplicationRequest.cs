using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class CreateApplicationRequest : IPostApiRequest
    {
        public int PledgeId { get; private set; }

        public CreateApplicationRequest(int pledgeId, long accountId, string details, string standardId,
            int numberOfApprentices, DateTime startDate, bool hasTrainingProvider, IEnumerable<string> sectors,
            string postcode, string firstName, string lastName, IEnumerable<string> emailAddresses,
            string businessWebsite)
        {
            PledgeId = pledgeId;
            Data = new CreateApplicationRequestData
            {
                EmployerAccountId = accountId,
                Details = details,
                StandardId = standardId,
                NumberOfApprentices = numberOfApprentices,
                StartDate = startDate,
                HasTrainingProvider = hasTrainingProvider,
                Sectors = sectors,
                Postcode = postcode,
                FirstName = firstName,
                LastName = lastName,
                EmailAddresses = emailAddresses,
                BusinessWebsite = businessWebsite
            };
        }
        
        public class CreateApplicationRequestData
        {
            public long EmployerAccountId { get; set; }

            public string Details { get; set; }

            public string StandardId { get; set; }
            public int NumberOfApprentices { get; set; }
            public DateTime StartDate { get; set; }
            public bool HasTrainingProvider { get; set; }

            public IEnumerable<string> Sectors { get; set; }
            public string Postcode { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public IEnumerable<string> EmailAddresses { get; set; }
            public string BusinessWebsite { get; set; }
        }

        public string PostUrl => $"/pledges/{PledgeId}/applications";
        public object Data { get; set; }
    }
}
