using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;

public class GetAccountFinanceQueryHandler(
        IAccountsService accountsService,
     IEmployerFinanceService employerFinanceService,
    IPayRefHashingService hashingService,
    IPayeSchemeObfuscator payeSchemeObfuscator,
    IDatetimeService datetimeService,
    ILogger<GetAccountFinanceQueryHandler> logger)
        : IRequestHandler<GetAccountFinanceQuery, GetAccountFinanceQueryResult>
{
    public async Task<GetAccountFinanceQueryResult> Handle(GetAccountFinanceQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Finance data for Account {account}", query.AccountId);

        var account = await accountsService.GetAccount(query.AccountId);
        if (account != null)
        {
            return await GetFinanceData(account);
        }

        return new GetAccountFinanceQueryResult();
    }

    private async Task<GetAccountFinanceQueryResult> GetFinanceData(Account account)
    {
        var payeSchemesTasks = GetPayeSchemeTasks(account);

        var transactionTasks = GetAccountTransactions(account.HashedAccountId);

        var balanceTask = GetAccountBalance(account.HashedAccountId);

        await Task.WhenAll(payeSchemesTasks, transactionTasks, balanceTask);

        return new GetAccountFinanceQueryResult
        {
            PayeSchemes = payeSchemesTasks.Result,
            Transactions = transactionTasks.Result,
            Balance = balanceTask.Result
        };
    }

    private async Task<IEnumerable<PayeScheme>> GetPayeSchemeTasks(Account account)
    {
        var mainPayeSchemes = await GetPayeSchemes(account);

        return mainPayeSchemes?.Select(payeScheme => new PayeScheme
        {
            Ref = payeScheme.Ref,
            DasAccountId = payeScheme.DasAccountId,
            AddedDate = payeScheme.AddedDate,
            RemovedDate = payeScheme.RemovedDate,
            Name = payeScheme.Name,
            HashedPayeRef = hashingService.HashValue(payeScheme.Ref),
            ObscuredPayeRef = payeSchemeObfuscator.ObscurePayeScheme(payeScheme.Ref)
        }).ToList();
    }

    private async Task<IEnumerable<PayeScheme>> GetPayeSchemes(Account account)
    {
        var payes = new List<PayeScheme>();

        var payesBatches = account.PayeSchemes
            .Select((item, inx) => new { item, inx })
            .GroupBy(x => x.inx / 50)
            .Select(g => g.Select(x => x.item));

        foreach (var payeBatch in payesBatches)
        {
            var payeTasks = payeBatch.Select(payeScheme =>
            {
                var obscured = payeSchemeObfuscator.ObscurePayeScheme(payeScheme.Id).Replace("/", "%252f");
                var paye = payeScheme.Id.Replace("/", "%252f");
                logger.LogDebug("IAccountApiClient.GetResource<PayeSchemeViewModel>(\"{Obscured}\");", payeScheme.Href.Replace(paye, obscured));

                return accountsService.GetEmployerAccountPayeScheme(payeScheme.Href).ContinueWith(payeTask =>
                {
                    if (!payeTask.IsFaulted)
                    {
                        return payeTask.Result;
                    }

                    logger.LogError(payeTask.Exception, "Exception occured in Account API type of {PayeSchemeViewModelName} at {PayeSchemeHref} id {PayeSchemeId}", nameof(PayeScheme), payeScheme.Href, payeScheme.Id);
                    return new PayeScheme();
                });
            });

            payes.AddRange(await Task.WhenAll(payeTasks));
        }

        return payes.Select(payeSchemeViewModel =>
        {
            if (!IsValidPayeScheme(payeSchemeViewModel))
            {
                return null;
            }

            var item = new PayeScheme
            {
                Ref = payeSchemeViewModel.Ref,
                DasAccountId = payeSchemeViewModel.DasAccountId,
                AddedDate = payeSchemeViewModel.AddedDate,
                RemovedDate = payeSchemeViewModel.RemovedDate,
                Name = payeSchemeViewModel.Name
            };

            return item;
        }).Where(x => x != null)
            .OrderBy(x => x.Ref);
    }

    private static bool IsValidPayeScheme(PayeScheme result)
    {
        return result.AddedDate <= DateTime.UtcNow &&
               (result.RemovedDate == null || result.RemovedDate > DateTime.UtcNow);
    }

    private async Task<List<Transaction>> GetAccountTransactions(string accountId)
    {
        var endDate = DateTime.Now.Date;
        var financialYearIterator = datetimeService.GetBeginningFinancialYear(new DateTime(2017, 4, 1));
        var response = new List<Transaction>();

        try
        {
            var transactions = await employerFinanceService.GetTransactions(accountId, financialYearIterator.Year,
                financialYearIterator.Month);
            response.AddRange(transactions);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occured in Finance API type of {TransactionsViewModel} for period {FinancialYearIteratorYear}.{FinancialYearIteratorMonth} id {AccountId}", nameof(TransactionsViewModel), financialYearIterator.Year, financialYearIterator.Month, accountId);
        }


        return GetFilteredTransactions(response);
    }

    private static List<Transaction> GetFilteredTransactions(IEnumerable<Transaction> response)
    {
        return response.Where(x => x.Description != null && x.Amount != 0).OrderByDescending(x => x.DateCreated).ToList();
    }

    private async Task<decimal> GetAccountBalance(string hashedAccountId)
    {
        try
        {
            var request = new GetAccountBalancesRequest([hashedAccountId]);
            var response = await employerFinanceService.GetAccountBalances(request);

            if (response == null || response.Body == null || response.Body.Count == 0)
            {
                return 0;
            }

            return response.Body.First().Balance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Account Balance with id {Id} not found", hashedAccountId);
            throw;
        }
    }
}