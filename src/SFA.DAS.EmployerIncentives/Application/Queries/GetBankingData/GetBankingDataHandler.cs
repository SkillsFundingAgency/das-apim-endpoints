using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData
{
    public class GetBankingDataHandler : IRequestHandler<GetBankingDataQuery, GetBankingDataResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;
        private readonly IAccountsService _accountsService;

        public GetBankingDataHandler(IEmployerIncentivesService employerIncentivesService, IAccountsService accountsService)
        {
            _employerIncentivesService = employerIncentivesService;
            _accountsService = accountsService;
        }
        public async Task<GetBankingDataResult> Handle(GetBankingDataQuery request, CancellationToken cancellationToken)
        {
            var application = await _employerIncentivesService.GetApplication(request.AccountId, request.ApplicationId);
            var legalEntity = await _accountsService.GetLegalEntity(request.HashedAccountId, application.LegalEntityId);

            var bankingData = new BankingData
            {
                VendorCode = "00000000",
                LegalEntityId = application.LegalEntityId,
                ApplicantEmail = "TODO", //TODO
                ApplicantName = "TODO", //TODO
                ApplicationValue = application.Apprenticeships.Sum(x => x.TotalIncentiveAmount),
                SignedAgreements = legalEntity.Agreements.Where(AgreementHasBeenSigned).Select(ToSignedAgreement)
            };

            return new GetBankingDataResult
            {
                Data = bankingData
            };
        }

        private static bool AgreementHasBeenSigned(Agreement agreement)
        {
            return agreement.Status == EmployerAgreementStatus.Signed || agreement.Status == EmployerAgreementStatus.Expired || agreement.Status == EmployerAgreementStatus.Superseded;
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