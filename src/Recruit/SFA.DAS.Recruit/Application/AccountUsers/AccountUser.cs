using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.AccountUsers
{
    public class AccountUser
    {
        public string DasAccountName { get; set; }
        public string EncodedAccountId { get; set; }
        public string Role { get; set; }
    }
}
