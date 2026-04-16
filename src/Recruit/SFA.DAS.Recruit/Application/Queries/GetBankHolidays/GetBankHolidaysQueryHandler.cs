using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.Application.Services;

namespace SFA.DAS.Recruit.Application.Queries.GetBankHolidays;

public class GetBankHolidaysQueryHandler(IBankHolidaysService bankHolidaysService) : IRequestHandler<GetBankHolidaysQuery, GetBankHolidaysQueryResult>
{
    public async Task<GetBankHolidaysQueryResult> Handle(GetBankHolidaysQuery request, CancellationToken cancellationToken)
    {
        var data = await bankHolidaysService.GetBankHolidayData();

        return new GetBankHolidaysQueryResult
        {
            Data = data
        };
    }
}