using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Models;

namespace SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;
public class ChallengeEntryCommandHandler(
    IAccountsService accountsService,
    IEmployerFinanceService financeService,
    IFinanceDataService financeDataService,
        IChallengeService challengeService,
    ILogger<ChallengeEntryCommandHandler> logger
    ) : IRequestHandler<ChallengeEntryCommand, ChallengeEntryCommandResult>
{
    public async Task<ChallengeEntryCommandResult> Handle(ChallengeEntryCommand command, CancellationToken cancellationToken)
    {
        var response = new ChallengeEntryCommandResult
        {
            Id = command.Id,
            IsValid = false,
            Characters = [command.FirstCharacterPosition, command.SecondCharacterPosition]
        };

        var isValidInput = !(string.IsNullOrEmpty(command.Balance)
                        || string.IsNullOrEmpty(command.Challenge1)
                        || string.IsNullOrEmpty(command.Challenge2)
                        || !int.TryParse(command.Balance.Split('.')[0].Replace("£", string.Empty), out _)
                        || command.Challenge1.Length != 1
                        || command.Challenge2.Length != 1
                       );

        if (!isValidInput)
        {
            return response;
        }

        var account = await accountsService.GetAccount(command.AccountId);
        var financeData = await financeDataService.GetFinanceData(account);

        var challengeResponse = challengeService.GetChallengeQueryResultFromAccount(account, financeData.PayeSchemes);

        if (challengeResponse.StatusCode != SearchResponseCodes.Success)
        {
            return response;
        }

        response.IsValid = await CheckData(financeData.PayeSchemes, command);

        return response;
    }

    private async Task<bool> CheckData(IEnumerable<PayeScheme> payeSchemes, ChallengeEntryCommand message)
    {
        var accountBalance = 0m;
        var accountBalances = await financeService.GetAccountBalances(new GetAccountBalancesRequest([message.Id]));
        if (accountBalances != null && accountBalances.Body != null && accountBalances.Body.Count > 0)
        {
            accountBalance = accountBalances.Body[0].Balance;
        }
        var validPayeSchemesData = CheckPayeSchemesData(payeSchemes, message);

        if (!decimal.TryParse(message.Balance.Replace("£", string.Empty), out decimal messageBalance))
        {
            return false;
        }

        var roundedAccountBalance = Math.Round(accountBalance);
        var roundedMessageBalance = Math.Round(messageBalance);

        logger.LogInformation("{TypeName}.{MethodName}: accountBalance: {AccountBalance}. messageBalance: {MessageBalance}. roundedAccountBalance: {RoundedAccountBalance}. roundedMessageBalance: {RoundedMessageBalance}",
            nameof(ChallengeEntryCommandHandler),
            nameof(CheckData),
            accountBalance,
            messageBalance,
            roundedAccountBalance,
            roundedMessageBalance
            );

        return roundedAccountBalance.Equals(roundedMessageBalance) && validPayeSchemesData;
    }

    private static bool CheckPayeSchemesData(IEnumerable<PayeScheme> recordPayeSchemes, ChallengeEntryCommand message)
    {
        var challengeInput = new List<string>
        {
            message.Challenge1.ToLower(),
            message.Challenge2.ToLower()
        };

        var list = recordPayeSchemes.Select(x => x.PayeRefWithOutSlash);
        var index1 = message.FirstCharacterPosition;
        var index2 = message.SecondCharacterPosition;

        return list.Any(x => x[index1].ToString().Equals(challengeInput[0], StringComparison.CurrentCultureIgnoreCase) &&
                             x[index2].ToString().Equals(challengeInput[1], StringComparison.CurrentCultureIgnoreCase));
    }
}