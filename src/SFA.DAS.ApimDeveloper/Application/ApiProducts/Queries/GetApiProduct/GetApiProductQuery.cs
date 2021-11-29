using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct
{
    public class GetApiProductQuery : IRequest<GetApiProductQueryResult>
    {
        public string ProductName { get ; set ; }
    }
}