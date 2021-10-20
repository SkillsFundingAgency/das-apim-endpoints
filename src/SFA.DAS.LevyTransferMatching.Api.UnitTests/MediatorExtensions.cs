using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using MediatR;
using Moq;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests
{
    public static class MediatorExtensions
    {
        public static void SetupMediatorResponseToReturnAsync<TResult, TQuery>(this Mock<IMediator> mediator, TResult result, Expression<Func<TQuery, bool>> parameterMatch)
            where TQuery : IRequest<TResult>
        {
            mediator.Setup(o => o.Send(It.IsAny<TQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);
        }
    }
}
