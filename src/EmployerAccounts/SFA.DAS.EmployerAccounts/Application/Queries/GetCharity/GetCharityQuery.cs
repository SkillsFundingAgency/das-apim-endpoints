using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCharity
{
    public class GetCharityQuery : IRequest<GetCharityResult>
    {
        public int RegistrationNumber { get; set; }
    }
}