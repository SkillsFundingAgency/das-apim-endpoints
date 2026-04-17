using MediatR;
using SFA.DAS.SharedOuterApi.Services;
using System.Threading;
using System.Threading.Tasks;

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