using AutoMapper;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            CreateMap<GetOverlappingTrainingDateResponse.ApprenticeshipOverlappingTrainingDateRequest, GetManageApprenticeshipDetailsResponse.ApprenticeshipOverlappingTrainingDateRequest>();
            CreateMap<GetManageApprenticeshipDetailsQueryResult, GetManageApprenticeshipDetailsResponse>();
        }
    }
}
