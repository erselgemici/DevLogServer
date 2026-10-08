using AutoMapper;
using DevLog.Application.Features.Comments.Commands;
using DevLog.Application.Features.Comments.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Comments.Mappings;

// AutoMapper profili, yorum entity'si ile dto/command arasındaki eşleşmeler
public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        // ReverseMap sayesinde iki yönlü de çalışıyor
        CreateMap<Comment, GetCommentsQueryResult>().ReverseMap();
        // yorumun altındaki cevaplar
        CreateMap<SubComment, CommentReplyResult>();
        CreateMap<Comment, GetCommentByIdQueryResult>().ReverseMap();
        CreateMap<Comment, CreateCommentCommand>().ReverseMap();
        CreateMap<Comment, UpdateCommentCommand>().ReverseMap();
    }
}
