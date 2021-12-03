using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProduct
{
    public class GetApiProductQuery : IRequest<GetApiProductQueryResult>
    {
        public string ProductId { get ; set ; }
        public string AccountType { get ; set ; }
        public string AccountIdentifier { get ; set ; }
    }
}