using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails
{
    public class GetLatestDetailsQuery : IRequest<GetLatestDetailsResult>
    {
        public string Identifier { get; set; }
        public OrganisationType OrganisationType { get; set; }
    }
}