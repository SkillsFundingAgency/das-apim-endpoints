using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerAccounts.InnerApi.Responses
{
    public class GetEnglishFractionResponse : List<GetEnglishFractionResponse.DasEnglishFraction>
    {
        public class DasEnglishFraction
        {
            public string Id { get; set; }
            public DateTime DateCalculated { get; set; }
            public decimal Amount { get; set; }
            public string EmpRef { get; set; }
        }
    }
}
