using System;
using MediatR;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData
{
    public class GetBankingDataQuery : IRequest<GetBankingDataResult>
    {
        public long AccountId { get ; set ; }
        
        public Guid ApplicationId { get; set; }
    }
}