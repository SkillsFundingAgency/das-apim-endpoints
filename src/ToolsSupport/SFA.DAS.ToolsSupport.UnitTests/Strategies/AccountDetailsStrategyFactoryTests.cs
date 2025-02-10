using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Models.Constants;
using SFA.DAS.ToolsSupport.Strategies;

namespace SFA.DAS.ToolsSupport.UnitTests.Strategies;

public class AccountDetailsStrategyFactoryTests
{
    [Test, MoqAutoData]
    public void CreateStrategy_ShouldReturn_LegalEntitiesStrategy_For_EmployerAccount(
          AccountDetailsStrategyFactory factory)
    {
        var selection = AccountFieldSelection.EmployerAccount;

        // Act
        var strategy = factory.CreateStrategy(selection);

        // Assert
        strategy.Should().BeOfType<AccountDetailsLegalEntitiesStrategy>();
    }


    [Test, MoqAutoData]
    public void CreateStrategy_ShouldThrowException_ForUnknown_AccountFieldSelection(
        AccountDetailsStrategyFactory factory)
    {
        Action act = () => factory.CreateStrategy(0);

        act.Should().Throw<InvalidOperationException>().WithMessage("No strategy found for AccountFieldSelection: 0");
    }
}
