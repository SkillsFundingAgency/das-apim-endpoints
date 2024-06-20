using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.Configuration
{
    public class CompaniesHouseApiConfiguration : ICompaniesHouseApiConfiguration
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }
}
