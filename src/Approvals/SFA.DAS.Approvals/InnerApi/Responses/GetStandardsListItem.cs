﻿using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public List<string> Options { get; set; }
        public StandardVersionDetail VersionDetail { get; set; }
        public string StandardPageUrl { get; set; }
        public string Route { get; set; }
        public string ApprenticeshipType { get; set; }
    }
}