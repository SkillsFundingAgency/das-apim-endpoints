using AutoMapper;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
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
            CreateMap<PaymentsStatus, GetManageApprenticeshipDetailsResponse.PaymentsStatusDetails>();
            CreateMap<GetManageApprenticeshipDetailsQueryResult, GetManageApprenticeshipDetailsResponse>();
            CreateMap<InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipsResponse, GetApprenticeshipsCSVQueryResult>();
            CreateMap<InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, GetApprenticeshipsCSVQueryResult.ApprenticeshipDetailsCSVResponse>();
            CreateMap<GetApprenticeshipsCSVQueryResult, PostApprenticeshipsCSVResponse>();
            CreateMap<GetApprenticeshipsCSVQueryResult.ApprenticeshipDetailsCSVResponse, PostApprenticeshipsCSVResponse.ApprenticeshipDetailsCSVResponse>();
            CreateMap<BulkUploadAddDraftApprenticeshipRequest, BulkUploadAddDraftApprenticeshipExtendedRequest>();
            CreateMap<InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipsResponse, GetApprenticeshipsQueryResult>();
            CreateMap<InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, GetApprenticeshipsQueryResult.ApprenticeshipDetailsResponse>();
            CreateMap<GetApprenticeshipsQueryResult, Models.Apprentices.GetApprenticeshipsResponse>();
            CreateMap<GetApprenticeshipsQueryResult.ApprenticeshipDetailsResponse, Models.Apprentices.GetApprenticeshipsResponse.ApprenticeshipDetailsResponse>();
            CreateMap<GetApprenticeshipsFilterValuesResponse, GetApprenticeshipsFilterValuesQueryResult>();
            CreateMap<GetApprenticeshipsFilterValuesQueryResult, GetApprenticeshipsFiltersResponse>();
        }
    }
}