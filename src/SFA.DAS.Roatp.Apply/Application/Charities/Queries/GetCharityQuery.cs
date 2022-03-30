using MediatR;

namespace SFA.DAS.Roatp.Apply.Application.Charities.Queries
{
    public class GetCharityQuery : IRequest<GetCharityResult>
    {
        public GetCharityQuery(int registrationNumber)
        {
            RegistrationNumber = registrationNumber;
        }
        public int RegistrationNumber { get; }
    }
}
