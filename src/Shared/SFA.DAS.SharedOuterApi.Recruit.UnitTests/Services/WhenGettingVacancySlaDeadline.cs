using System.Text.Json;
using SFA.DAS.SharedOuterApi.Recruit.Services;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests.Services;

public class WhenGettingVacancySlaDeadline
{
    private readonly BankHolidaysData _bankHolidaysData = JsonSerializer.Deserialize<BankHolidaysData>(TestData.BankHolidaysJson)!;

    [Test, MoqAutoData]
    public async Task GetSlaDeadlineAsync_ShouldHandleWorkingDays(
        [Frozen] Mock<IBankHolidaysService> bankHolidaysService,
        [Greedy] VacancySlaDeadlineService sut)
    {
        // arrange
        bankHolidaysService
            .Setup(b => b.GetBankHolidayDataAsync())
            .ReturnsAsync(_bankHolidaysData);

        // act
        var actual = await sut.GetSlaDeadlineAsync(DateTime.Parse("2026-01-05 14:38"));

        // assert
        actual.Should().Be(DateTime.Parse("2026-01-06 14:38"));
    }

    [Test, MoqAutoData]
    public async Task GetSlaDeadlineAsync_ShouldHandleWeekends(
        [Frozen] Mock<IBankHolidaysService> bankHolidaysService,
        [Greedy] VacancySlaDeadlineService sut)
    {
        // arrange
        bankHolidaysService
            .Setup(b => b.GetBankHolidayDataAsync())
            .ReturnsAsync(_bankHolidaysData);
    
        // act
        var actual = await sut.GetSlaDeadlineAsync(DateTime.Parse("2026-01-02 14:38"));
    
        // assert
        actual.Should().Be(DateTime.Parse("2026-01-05 14:38"));
    }
    
    [Test, MoqAutoData]
    public async Task GetSlaDeadlineAsync_ShouldHandleBankHolidays(
        [Frozen] Mock<IBankHolidaysService> bankHolidaysService,
        [Greedy] VacancySlaDeadlineService sut)
    {
        // arrange
        bankHolidaysService
            .Setup(b => b.GetBankHolidayDataAsync())
            .ReturnsAsync(_bankHolidaysData);

        // act
        var actual = await sut.GetSlaDeadlineAsync(DateTime.Parse("2026-05-1 14:38"));

        // assert
        actual.Should().Be(DateTime.Parse("2026-05-5 14:38"));
    }
    
    [Test, MoqAutoData]
    public async Task GetSlaDeadlineAsync_ShouldHandleEaster(
        [Frozen] Mock<IBankHolidaysService> bankHolidaysService,
        [Greedy] VacancySlaDeadlineService sut)
    {
        // arrange
        bankHolidaysService
            .Setup(b => b.GetBankHolidayDataAsync())
            .ReturnsAsync(_bankHolidaysData);
    
        // act
        var actual = await sut.GetSlaDeadlineAsync(DateTime.Parse("2026-04-02 14:38"));
    
        // assert
        actual.Should().Be(DateTime.Parse("2026-04-07 14:38"));
    }
    
    [Test, MoqAutoData]
    public async Task GetSlaDeadlineAsync_ShouldHandleSubmissionsOnNonWorkingDays(
        [Frozen] Mock<IBankHolidaysService> bankHolidaysService,
        [Greedy] VacancySlaDeadlineService sut)
    {
        // arrange
        bankHolidaysService
            .Setup(b => b.GetBankHolidayDataAsync())
            .ReturnsAsync(_bankHolidaysData);
    
        // act
        var actual = await sut.GetSlaDeadlineAsync(DateTime.Parse("2026-05-02 14:38"));
    
        // assert
        actual.Should().Be(DateTime.Parse("2026-05-06 00:00"));
    }
}