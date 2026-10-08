using AutoMapper;
using DevLog.Application.Features.SubComments.Commands;
using DevLog.Application.Features.SubComments.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.SubComments.Mappings;

// AutoMapper profili, alt yorum entity'si ile dto/command arasındaki eşleşmeler
public class SubCommentMappingProfile : Profile
{
    public SubCommentMappingProfile()
    {
        // ReverseMap sayesinde iki yönlü de çalışıyor
        CreateMap<SubComment, GetSubCommentsQueryResult>().ReverseMap();
        CreateMap<SubComment, GetSubCommentByIdQueryResult>().ReverseMap();
        CreateMap<SubComment, CreateSubCommentCommand>().ReverseMap();
        CreateMap<SubComment, UpdateSubCommentCommand>().ReverseMap();
    }
}
