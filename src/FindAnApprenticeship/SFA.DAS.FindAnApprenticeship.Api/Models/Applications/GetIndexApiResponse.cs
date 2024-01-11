using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetIndexApiResponse
    {
        public string VacancyTitle { get; set; }

        public static implicit operator GetIndexApiResponse(GetIndexQueryResult source)
        {
            return new GetIndexApiResponse
            {
                VacancyTitle = source.VacancyTitle
            };
        }
    }
}
