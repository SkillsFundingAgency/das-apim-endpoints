using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ApprenticeData
{
    public class GetChangeOfEmployerApprenticeDataQueryResult
    {
        public GetApprenticeshipResponse Apprenticeship { get; set; }
        public GetPriceEpisodesResponse PriceEpisodes { get; set; }
        public GetAccountLegalEntityResponse AccountLegalEntity { get; set; }
        public GetTrainingProgrammeResponse TrainingProgrammeResponse { get; set; }
    }
}
