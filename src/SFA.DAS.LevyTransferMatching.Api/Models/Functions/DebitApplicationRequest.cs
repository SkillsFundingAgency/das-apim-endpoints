using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class DebitApplicationRequest
    {
        public int ApplicationId { get; set; }
        public int NumberOfApprentices { get; set; }
        public int Amount { get; set; }
    }
}
