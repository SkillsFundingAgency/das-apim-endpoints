using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using static System.String;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
    }
}
