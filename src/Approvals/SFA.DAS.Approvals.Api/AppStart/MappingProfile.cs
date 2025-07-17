using AutoMapper;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetPriceEpisodesResponse;
using GetApprenticeshipUpdatesResponse = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;

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
            CreateMap<PaymentsStatus, GetManageApprenticeshipDetailsResponse.PaymentsStatusDetails>();
            CreateMap<GetApprenticeshipsResponse, GetApprenticeshipsCSVQueryResult>();
            CreateMap<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, GetApprenticeshipsCSVQueryResult.ApprenticeshipDetailsCSVResponse>();
            CreateMap<GetApprenticeshipsCSVQueryResult, PostApprenticeshipsCSVResponse>();
            CreateMap<GetApprenticeshipsCSVQueryResult.ApprenticeshipDetailsCSVResponse, PostApprenticeshipsCSVResponse.ApprenticeshipDetailsCSVResponse>();
            CreateMap<Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails.LearnerStatusDetails, Models.Apprentices.LearnerStatusDetails>();
            CreateMap<BulkUploadAddDraftApprenticeshipRequest, BulkUploadAddDraftApprenticeshipExtendedRequest>();
        }
    }
}
