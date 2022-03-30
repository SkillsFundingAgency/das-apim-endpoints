using SFA.DAS.Roatp.Apply.Domain.Models;

namespace SFA.DAS.Roatp.Apply.Application.Charities.Queries
{
    public class GetCharityResult
    {
        public Charity Charity { get; }
        public GetCharityResult(Charity charity)
        {
            Charity = charity;
        }
    }
}
