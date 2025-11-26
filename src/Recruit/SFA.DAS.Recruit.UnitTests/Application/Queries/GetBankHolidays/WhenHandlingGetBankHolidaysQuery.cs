using SFA.DAS.Recruit.Application.Queries.GetBankHolidays;
using SFA.DAS.Recruit.Application.Services;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetBankHolidays;

public class WhenHandlingGetBankHolidaysQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_BankHoliday_Data_Returned(
        GetBankHolidaysQuery query,
        BankHolidaysData data,
        [Frozen] Mock<IBankHolidaysService> bankHolidaysService,
        GetBankHolidaysQueryHandler handler)
    {
        bankHolidaysService.Setup(x => x.GetBankHolidayData()).ReturnsAsync(data);
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Data.Should().BeEquivalentTo(data);
    }
}