﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Pledge : PledgeReference
    {
        public long AccountId { get; set; }
        public int Amount { get; set; }
        public int RemainingAmount { get; set; }
        public bool IsNamePublic { get; set; }
        public string DasAccountName { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> Levels { get; set; }
        public List<LocationDataItem> Locations { get; set; }
    }
}