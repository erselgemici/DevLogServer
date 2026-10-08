using AutoMapper;
using DevLog.Application.Features.Messages.Commands;
using DevLog.Application.Features.Messages.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Mappings;

// AutoMapper profili, mesaj entity'si ile dto/command arasındaki eşleşmeler
public class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        // ReverseMap sayesinde iki yönlü de çalışıyor
        CreateMap<Message, GetMessagesQueryResult>().ReverseMap();
        CreateMap<Message, GetMessageByIdQueryResult>().ReverseMap();
        CreateMap<Message, GetReadMessagesQueryResult>().ReverseMap();
        CreateMap<Message, GetUnreadMessagesQueryResult>().ReverseMap();
        CreateMap<Message, CreateMessageCommand>().ReverseMap();
        CreateMap<Message, UpdateMessageCommand>().ReverseMap();
    }
}
