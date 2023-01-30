using MediatR;

namespace SFA.DAS.Funding.Application.Queries.GetProviderAcademicYearEarnings
{
    public class GetProviderAcademicYearEarningsQuery : IRequest<GetProviderAcademicYearEarningsResult>
    {
        public long Ukprn { get ; set ; }
    }
}