using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.ContactInfos.Commands;

// iletişim bilgisi güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public record UpdateContactInfoCommand : IRequest<BaseResult<object>>
{
    public Guid Id { get; init; }
    public string Address { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string MapUrl { get; init; }
}
