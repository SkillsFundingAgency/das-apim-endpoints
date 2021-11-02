﻿using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Responses
{
    public class GetApplicationResponse
    {
        public string Postcode { get; set; }
        public string StandardId { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string Details { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }
        public string PledgeEmployerAccountName { get; set; }
        public IEnumerable<string> PledgeSectors { get; set; }
        public IEnumerable<string> PledgeLevels { get; set; }
        public IEnumerable<string> PledgeJobRoles { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public int Amount { get; set; }
        public List<PledgeLocation> PledgeLocations { get; set; }
        public string Status { get; set; }
        public bool PledgeIsNamePublic { get; set; }
        public int PledgeId { get; set; }
        public int PledgeAmount { get; set; }
       public long SenderEmployerAccountId { get; set; }
        public int AmountUsed { get; set; }
        public int NumberOfApprenticesUsed { get; set; }

        public class PledgeLocation
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}