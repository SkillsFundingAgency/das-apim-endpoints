using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProducts
{
    public class GetApiProductsQuery : IRequest<GetApiProductsQueryResult>
    {
        public string AccountType { get ; set ; }
        public string AccountIdentifier { get ; set ; }
    }
}