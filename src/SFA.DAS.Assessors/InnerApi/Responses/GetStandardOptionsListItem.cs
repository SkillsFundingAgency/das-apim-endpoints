﻿using System.Collections.Generic;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardOptionsListItem
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string IfateReferenceNumber { get; set; }
        public decimal Version { get; set; }
        public List<string> Options { get; set; }
    }
}
