using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData
{
    public class GetBankingDataHandler : IRequestHandler<GetBankingDataQuery, GetBankingDataResult>
    {
        private readonly IApplicationService _applicationService;
        private readonly IAccountsService _accountsService;

        public GetBankingDataHandler(IApplicationService applicationService, IAccountsService accountsService)
        {
            _applicationService = applicationService;
            _accountsService = accountsService;
        }
        public async Task<GetBankingDataResult> Handle(GetBankingDataQuery request, CancellationToken cancellationToken)
        {
            var application = await _applicationService.Get(request.AccountId, request.ApplicationId);
            if (application == null) throw new ArgumentException("Requested application details cannot be found in SFA.DAS.EmployerIncentives Application Service");

            var legalEntity = await _accountsService.GetLegalEntity(request.HashedAccountId, application.LegalEntityId);
            if (legalEntity == null) throw new ArgumentException("Requested legal entity details cannot be found in SFA.DAS.EAS Employer Accounts Service");

            var bankingData = new BankingData
            {
                VendorCode = "00000000",
                LegalEntityId = application.LegalEntityId,
                SubmittedByEmail = application.SubmittedByEmail,
                SubmittedByName = application.SubmittedByName,
                ApplicationValue = application.Apprenticeships.Sum(x => x.TotalIncentiveAmount),
                NumberOfApprenticeships = application.Apprenticeships.Count(),
                SignedAgreements = legalEntity.Agreements.Where(AgreementHasBeenSigned).Select(ToSignedAgreement)
            };

            return new GetBankingDataResult
            {
                Data = bankingData
            };
        }

        private static bool AgreementHasBeenSigned(Agreement agreement)
        {
            return (agreement.Status == EmployerAgreementStatus.Signed || agreement.Status == EmployerAgreementStatus.Expired || agreement.Status == EmployerAgreementStatus.Superseded) && agreement.SignedDate.HasValue;
        }

        private SignedAgreement ToSignedAgreement(Agreement agreement)
        {
            return new SignedAgreement
            {
                SignedByEmail = agreement.SignedByEmail,
                SignedByName = agreement.SignedByName,
                SignedDate = agreement.SignedDate.Value
            };
        }
    }
}