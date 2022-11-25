using System;
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

        public GetProviderAcademicYearEarningsHandler(IFundingProviderEarningsService providerEarningsService)
        {
            _providerEarningsService = providerEarningsService;
        }

        public async Task<GetProviderAcademicYearEarningsResult> Handle(GetProviderAcademicYearEarningsQuery request, CancellationToken cancellationToken)
        {
            var academicYearEarnings = await _providerEarningsService.GetAcademicYearEarnings(request.Ukprn);

            var earningsToReturn = MapAcademicYearEarnings(academicYearEarnings);

            return new GetProviderAcademicYearEarningsResult
            {
                AcademicYearEarnings = earningsToReturn
            };
        }

        private AcademicYearEarnings MapAcademicYearEarnings(AcademicYearEarningsDto earningsDto)
        {
            return new AcademicYearEarnings
            {
                Learners = earningsDto.Learners.Select(MapLearner).ToList()
            };
        }

        private Learner MapLearner(LearnerDto learnerDto)
        {
            return new Learner
            {
                FundingType = (Models.FundingType)Enum.Parse(typeof(Models.FundingType), learnerDto.FundingType.ToString()),
                OnProgrammeEarnings = learnerDto.OnProgrammeEarnings.Select(MapOnProgrammeEarnings).ToList(),
                TotalOnProgrammeEarnings = learnerDto.TotalOnProgrammeEarnings,
                Uln = learnerDto.Uln
            };
        }

        private OnProgrammeEarning MapOnProgrammeEarnings(OnProgrammeEarningDto earningDto)
        {
            return new OnProgrammeEarning
            {
                AcademicYear = earningDto.AcademicYear,
                Amount = earningDto.Amount,
                DeliveryPeriod = earningDto.DeliveryPeriod
            };
        }
    }
}