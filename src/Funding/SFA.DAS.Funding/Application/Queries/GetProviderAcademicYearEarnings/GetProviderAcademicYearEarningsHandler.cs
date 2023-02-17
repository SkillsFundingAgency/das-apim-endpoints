using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Application.Queries.GetProviderAcademicYearEarnings
{
    public class GetProviderAcademicYearEarningsHandler : IRequestHandler<GetProviderAcademicYearEarningsQuery, GetProviderAcademicYearEarningsResult>
    {
        private readonly IFundingProviderEarningsService _providerEarningsService;
        private readonly IApprenticeshipsService _apprenticeshipsService;

        public GetProviderAcademicYearEarningsHandler(IFundingProviderEarningsService providerEarningsService, IApprenticeshipsService apprenticeshipsService)
        {
            _providerEarningsService = providerEarningsService;
            _apprenticeshipsService = apprenticeshipsService;
        }

        public async Task<GetProviderAcademicYearEarningsResult> Handle(GetProviderAcademicYearEarningsQuery request, CancellationToken cancellationToken)
        {
            var academicYearEarnings = await _providerEarningsService.GetAcademicYearEarnings(request.Ukprn);
            var apprenticeships = await _apprenticeshipsService.GetAll(request.Ukprn);

            var earningsResult = MapAcademicYearEarnings(academicYearEarnings, apprenticeships);

            return new GetProviderAcademicYearEarningsResult
            {
                AcademicYearEarnings = earningsResult
            };
        }

        private AcademicYearEarnings MapAcademicYearEarnings(AcademicYearEarningsDto earningsDto, ApprenticeshipsDto apprenticeshipDto)
        {
            var earnings = new AcademicYearEarnings() { Learners = new List<Learner>()};
            foreach (var learner in earningsDto.Learners)
            {
                var apprenticeship = apprenticeshipDto.Apprenticeships.Find(x => x.Uln == learner.Uln);
                if (apprenticeship is null)
                {
                    continue;
                }

                earnings.Learners.Add(new Learner()
                {
                    FundingType = (Models.FundingType)Enum.Parse(typeof(Models.FundingType), learner.FundingType.ToString()),
                    OnProgrammeEarnings = learner.OnProgrammeEarnings.Select(MapOnProgrammeEarnings).ToList(),
                    TotalOnProgrammeEarnings = learner.TotalOnProgrammeEarnings,
                    Uln = learner.Uln,
                    FirstName = apprenticeship.FirstName,
                    LastName = apprenticeship.LastName
                });
            }
            return earnings;
        }

        private OnProgrammeEarning MapOnProgrammeEarnings(OnProgrammeEarningDto earningDto)
        {
            return new OnProgrammeEarning
            {
                AcademicYear = earningDto.AcademicYear,
                Amount = earningDto.Amount,
                DeliveryPeriod = earningDto.DeliveryPeriod,
                EmployerContribution = earningDto.EmployerContribution,
                GovernmentContribution = earningDto.GovernmentContribution
            };
        }
    }
}