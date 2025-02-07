using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Strategies;

public interface IAccountDetailsStrategyFactory
{
    IAccountDetailsStrategy CreateStrategy(AccountFieldSelection selectedField);

}
public class AccountDetailsStrategyFactory : IAccountDetailsStrategyFactory
{
    private readonly Dictionary<AccountFieldSelection, Func<IAccountDetailsStrategy>> _strategyFactories;

    public AccountDetailsStrategyFactory(
        IAccountsService accountService,
        ILogger logger)
    {
        _strategyFactories = new Dictionary<AccountFieldSelection, Func<IAccountDetailsStrategy>>
            {
                { AccountFieldSelection.EmployerAccount, () => new AccountDetailsLegalEntitiesStrategy(accountService, logger) }
            };
    }

    public IAccountDetailsStrategy CreateStrategy(AccountFieldSelection selectedField)
    {
        if (_strategyFactories.TryGetValue(selectedField, out var strategyFactory))
        {
            return strategyFactory.Invoke();
        }

        throw new InvalidOperationException($"No strategy found for AccountFieldSelection: {selectedField}");
    }
}

