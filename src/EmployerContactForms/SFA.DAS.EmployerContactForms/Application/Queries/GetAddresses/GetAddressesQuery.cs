using MediatR;

namespace SFA.DAS.EmployerContactForms.Application.Queries.GetAddresses
{
    public class GetAddressesQuery : IRequest<GetAddressesQueryResult>
    {
        public string Query { get; }

        public GetAddressesQuery(string query)
        {
            Query = query;
        }
    }
}
