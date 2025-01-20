using AutoMapper;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;
using SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;

namespace SFA.DAS.AODP.AutoMapper.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Forms Request Mapping
        CreateMap<CreateFormVersionCommand.FormVersion, CreateFormVersionApiRequest.FormVersion>().ReverseMap();
        CreateMap<UpdateFormVersionCommand.FormVersion, UpdateFormVersionApiRequest.FormVersion>().ReverseMap();
        #endregion

        #region Forms Response Mapping
        CreateMap<GetAllFormVersionsQueryResponse.FormVersion, GetAllFormVersionsApiResponse.FormVersion>().ReverseMap();
        CreateMap<GetFormVersionByIdQueryResponse.FormVersion, GetFormVersionByIdApiResponse.FormVersion>().ReverseMap();
        CreateMap<CreateFormVersionCommandResponse.FormVersion, CreateFormVersionApiResponse.FormVersion>().ReverseMap();
        CreateMap<UpdateFormVersionCommandResponse.FormVersion, UpdateFormVersionApiResponse.FormVersion>().ReverseMap();
        #endregion

        #region Sections Request Mapping
        CreateMap<CreateSectionCommand.Section, CreateSectionApiRequest.Section>().ReverseMap();
        CreateMap<UpdateSectionCommand.Section, UpdateSectionApiRequest.Section>().ReverseMap();
        #endregion

        #region Sections Response Mapping
        CreateMap<GetAllSectionsQueryResponse.Section, GetAllSectionsApiResponse.Section>().ReverseMap();
        CreateMap<GetSectionByIdQueryResponse.Section, GetSectionByIdApiResponse.Section>().ReverseMap();
        CreateMap<CreateSectionCommandResponse.Section, CreateSectionApiResponse.Section>().ReverseMap();
        CreateMap<UpdateSectionCommandResponse.Section, UpdateSectionApiResponse.Section>().ReverseMap();
        #endregion

        #region Pages Request Mapping
        CreateMap<CreatePageCommand.Page, CreatePageApiRequest.Page>().ReverseMap();
        CreateMap<UpdatePageCommand.Page, UpdatePageApiRequest.Page>().ReverseMap();
        #endregion

        #region Pages Response Mapping
        CreateMap<GetAllPagesQueryResponse.Page, GetAllSectionsApiResponse.Section>().ReverseMap();
        CreateMap<GetPageByIdQueryResponse.Page, GetPageByIdApiResponse.Page>().ReverseMap();
        CreateMap<CreatePageCommandResponse.Page, CreatePageApiResponse.Page>().ReverseMap();
        CreateMap<UpdatePageCommandResponse.Page, UpdatePageApiResponse.Page>().ReverseMap();
        #endregion
    }
}
