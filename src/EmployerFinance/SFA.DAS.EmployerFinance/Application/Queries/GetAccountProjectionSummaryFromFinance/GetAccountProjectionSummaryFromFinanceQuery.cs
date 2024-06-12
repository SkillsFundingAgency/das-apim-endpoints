using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummaryFromFinance
{
    public class GetAccountProjectionSummaryFromFinanceQuery : IRequest<GetAccountProjectionSummaryFromFinanceResult>
    {
        public long AccountId { get; set; }
    }
}
