using AutoMapper;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.Approvals.Api.AppStart
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetApprenticeshipResponse, GetManageApprenticeshipDetailsResponse.ApprenticeshipDetails>();
            CreateMap<PriceEpisode, GetManageApprenticeshipDetailsResponse.PriceEpisode>();
            CreateMap<GetApprenticeshipUpdatesResponse.ApprenticeshipUpdate, GetManageApprenticeshipDetailsResponse.ApprenticeshipUpdate>();
            CreateMap<GetDataLocksResponse.DataLock, GetManageApprenticeshipDetailsResponse.DataLock>();
            CreateMap<GetChangeOfPartyRequestsResponse.ChangeOfPartyRequest, GetManageApprenticeshipDetailsResponse.ChangeOfPartyRequest>();
            CreateMap<GetChangeOfProviderChainResponse.ChangeOfProviderLink, GetManageApprenticeshipDetailsResponse.ChangeOfProviderLink>();
            CreateMap<GetChangeOfEmployerChainResponse.ChangeOfEmployerLink, GetManageApprenticeshipDetailsResponse.ChangeOfEmployerLink>();
            CreateMap<GetOverlappingTrainingDateResponse.ApprenticeshipOverlappingTrainingDateRequest, GetManageApprenticeshipDetailsResponse.ApprenticeshipOverlappingTrainingDateRequest>();
            CreateMap<PendingPriceChange, GetManageApprenticeshipDetailsResponse.PendingPriceChangeDetails>();
            CreateMap<PendingStartDateChange, GetManageApprenticeshipDetailsResponse.PendingStartDateChangeDetails>();
            CreateMap<GetManageApprenticeshipDetailsQueryResult, GetManageApprenticeshipDetailsResponse>();
        }
    }
}
