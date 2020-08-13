using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData
{
    public class GetBankingDataHandler : IRequestHandler<GetBankingDataQuery, GetBankingDataResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetBankingDataHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<GetBankingDataResult> Handle(GetBankingDataQuery request, CancellationToken cancellationToken)
        {
            var application = await _employerIncentivesService.GetApplication(request.AccountId, request.ApplicationId);

            var bankingData = new BankingData
            {
                VendorCode = "00000000",
                LegalEntityId = application.LegalEntityId,
                ApplicantEmail = "TODO", //TODO
                ApplicantName = "TODO", //TODO
                ApplicationValue = application.Apprenticeships.Sum(x => x.TotalIncentiveAmount)
            };

            return new GetBankingDataResult
            {
                Data = bankingData
            };
        }
    }
}